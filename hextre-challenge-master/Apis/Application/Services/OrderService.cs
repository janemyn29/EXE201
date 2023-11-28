using Application.Interfaces;
using Application.Services.Momo;
using Application.Services.Vnpay;
using Application.ViewModels.OrderViewModels;
using Domain.Entities;
using Domain.Enums;
using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public OrderService(IUnitOfWork unit, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _unit = unit;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<Guid> CreateOrder(OrderCreateModel model)
        {
            var Customer = await _userManager.Users.SingleOrDefaultAsync(x => x.Id.ToLower().Equals(model.CustomerId.ToLower()));
            if (Customer == null)
            {
                throw new Exception("Không tìm thấy khách hàng bạn yêu cầu");
            }
            var warehouseDetail = await _unit.WarehouseDetailRepository.GetByIdAsync(model.WarehouseDetailId);
            if (warehouseDetail == null)
            {
                throw new Exception("Không tìm thấy chi tiết kho bạn yêu cầu");
            }
            if (warehouseDetail.Quantity <= 0)
            {
                throw new Exception("Kho này hiện đang hết. Vui lòng đặt lại sau!");
            }
            var order = new Order();
            order.CustomerId = model.CustomerId;
            order.WarehouseDetailId = model.WarehouseDetailId;
            order.ContactInDay = false;
            order.TotalCall = 0;
            order.OrderStatus = OrderStatus.Pending;
            order.WarehousePrice = warehouseDetail.WarehousePrice;
            order.ServicePrice = warehouseDetail.ServicePrice;
            order.Deposit = warehouseDetail.WarehousePrice + warehouseDetail.ServicePrice;
            order.TotalPrice = order.Deposit *2;
            
            order.Width = warehouseDetail.Width;
            order.Height = warehouseDetail.Height;
            order.Depth = warehouseDetail.Depth;
            order.UnitType = warehouseDetail.UnitType;
            order.PaymentStatus = PaymentStatus.Waiting;
            order.CreationDate = DateTime.Now;

            await _unit.OrderRepository.AddAsync(order);
            await _unit.SaveChangeAsync();
            return order.Id;
        }

        public async Task<string> Payment(Order order, string option)
        {
            string endpoint = _configuration["MomoServices:endpoint"];
            string partnerCode = _configuration["MomoServices:partnerCode"];
            string accessKey = _configuration["MomoServices:accessKey"];
            string serectkey = _configuration["MomoServices:secretKey"];
            string orderInfo = "Thanh toán đơn hàng tại WarehouseBridge.";
            string redirectUrl = _configuration["MomoServices:redirectUrl"];
            string ipnUrl = _configuration["MomoServices:ipnUrl"];
            string requestType = option;
            string amount = order.TotalPrice.ToString();
            string orderId = order.Id.ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "Thanh toán đơn hàng tại WarehouseBridge.";
            //captureWallet


            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
            ;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);



            //build body json request
            JObject message = new JObject
                 {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

                };
            try
            {
                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

                JObject jmessage = JObject.Parse(responseFromMomo);

                return jmessage.GetValue("payUrl").ToString();
            }
            catch
            {
                throw new Exception("Đã xảy ra lối trong qua trình thanh toán. Vui lòng thanh toán lại sau!");
            }
        }

        private static readonly ILog log =
          LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async Task<string> PaymentVNPAY(/*Order temp*/ string ip)
        {

            /*
              <add key="vnp_Url" value="https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"/>
    <add key="vnp_Api" value="https://sandbox.vnpayment.vn/merchant_webapi/api/transaction"/>
    <add key="vnp_TmnCode" value="S7SMHMU1"/>
    <add key="vnp_HashSecret" value="TZXGTOSTHYQNHATPAFVEHWZVBBBZLXPZ"/>
    <add key="vnp_Returnurl" value="http://localhost:16262/vnpay_return.aspx"/>*/
            string vnp_Returnurl = "https://www.youtube.com/"; //URL nhan ket qua tra ve 
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = "S7SMHMU1"; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = "TZXGTOSTHYQNHATPAFVEHWZVBBBZLXPZ"; //Secret Key


            OrderInfo order = new OrderInfo();
            order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.Amount = 100000; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
            order.CreatedDate = DateTime.Now;


            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr",ip);
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");


            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "250000"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_IpnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            //Billing



            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return paymentUrl;
        }


    }
}
