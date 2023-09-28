using Application;
using Application.Interfaces;
using Application.Repositories;
using Application.Services.Momo;
using Application.ViewModels.OrderViewModels;
using AutoMapper;
using Domain.Entities;
using Infrastructures;
using Infrastructures.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Authorize(Roles =("Customer"))]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly IOrderService _orderService;
        private readonly IClaimsService _claimsService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly INoteRepository _noteRepository;
        private readonly IConfiguration _configuration;

        public OrdersController(IUnitOfWork unit, IOrderService orderService,IClaimsService claimsService,AppDbContext context,IMapper mapper, INoteRepository noteRepository, IConfiguration configuration)
        {
            _unit = unit;
            _orderService = orderService;
            _claimsService = claimsService;
            _context = context;
            _mapper = mapper;
            _noteRepository = noteRepository;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<ActionResult> GetOrder()
        {
            var id = _claimsService.GetCurrentUserId.ToString();
            var orders = await _context.Order.AsNoTracking().Where(x => x.IsDeleted == false && x.CustomerId.ToLower().Equals(id.ToLower())).ToListAsync();
            var result = _mapper.Map<IList<OrderViewModel>>(orders);
            return Ok(result);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrder(Guid id)
        {
            if (_context.Order == null)
            {
                return NotFound();
            }
            var userId = _claimsService.GetCurrentUserId.ToString(); 
            var category = await _context.Order.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false && x.CustomerId.ToLower().Equals(userId.ToLower()));

            if (category == null)
            {
                return NotFound("Không tìm thấy đơn hàng kho bạn yêu cầu!");
            }
            var result = _mapper.Map<OrderViewModel>(category);
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostOrder(Guid warehouseDetailId)
        {
            try
            {
                var ordermodel = new OrderCreateModel();
                ordermodel.CustomerId =_claimsService.GetCurrentUserId.ToString();
                ordermodel.WarehouseDetailId =warehouseDetailId;
                var orderId = await _orderService.CreateOrder(ordermodel);
                var note = new Notification();
                note.ApplicationUserId = ordermodel.CustomerId;
                note.IsRead = false;
                note.Title = "Đơn hàng mới";
                note.Description = "Bạn vừa tạo đơn hàng thành công. Xin vui lòng thanh toán!";
                await _unit.NoteRepository.AddAsync(note);
                var warehouseDetail = await _unit.WarehouseDetailRepository.GetByIdAsync(warehouseDetailId);
                warehouseDetail.Quantity--;
                _unit.WarehouseDetailRepository.Update(warehouseDetail);
                await _unit.SaveChangeAsync();
                var order = await _unit.OrderRepository.GetByIdAsync(orderId);
                string returnUrl = await _orderService.Payment(order);
                return Ok(returnUrl);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("IpnHandler")]
        public async Task IpnHandler([FromBody] MomoRedirect momo)
        {
            var tempPayType = momo.payType;
            if (momo.payType == null)
            {
                tempPayType = "none";
            }

            string endpoint = _configuration["MomoServices:endpoint"];
            string partnerCode = _configuration["MomoServices:partnerCode"];
            string accessKey = _configuration["MomoServices:accessKey"];
            string serectkey = _configuration["MomoServices:serectkey"];
            string redirectUrl = _configuration["MomoServices:redirectUrl"];
            string ipnUrl = _configuration["MomoServices:ipnUrl"];

            string rawHash = "accessKey=" + accessKey +
                    "&amount=" + momo.amount +
                    "&extraData=" + momo.extraData +
                    "&message=" + momo.message +
                    "&orderId=" + momo.orderId +
                    "&orderInfo=" + momo.orderInfo +
                    "&orderType=" + momo.orderType +
                    "&partnerCode=" + partnerCode +
                    "&payType=" + momo.payType +
                    "&requestId=" + momo.requestId +
                    "&responseTime=" + momo.responseTime +
                    "&resultCode=" + momo.resultCode +
                    "&transId=" + momo.transId;

            //hash rawData
            MoMoSecurity crypto = new MoMoSecurity();
            string temp = crypto.signSHA256(rawHash, serectkey);
            //check chữ ký
            if (temp != momo.signature)
            {
                return;
            }
            try
            {
                DepositPayment deposit = new DepositPayment();
                deposit.OrderId = Guid.Parse(momo.orderId);

                deposit.Amount = momo.amount;
                deposit.IpnURL = ipnUrl;
                deposit.OrderInfo = momo.orderInfo;
                deposit.PartnerCode = partnerCode;
                deposit.RedirectUrl = redirectUrl;
                deposit.RequestId = momo.requestId;
                deposit.RequestType = "captureWallet";
                if(momo.resultCode == 0)
                {
                    deposit.Status = Domain.Enums.DepositStatus.Success;
                }
                else
                {
                    deposit.Status = Domain.Enums.DepositStatus.Failed;
                }

                deposit.PaymentMethod = "Momo";
                deposit.orderIdFormMomo = momo.orderId;
                deposit.orderType = momo.orderType;
                deposit.transId = momo.transId;
                deposit.resultCode = momo.resultCode;
                deposit.message = momo.message;
                deposit.payType = tempPayType;
                deposit.responseTime = momo.responseTime;
                deposit.extraData = momo.extraData;
                deposit.signature = momo.signature;
                await _unit.DepositRepository.AddAsync(deposit);
                await _unit.SaveChangeAsync();

                var order = await _unit.OrderRepository.GetByIdAsync(Guid.Parse(momo.orderId));
                if(momo.resultCode == 0)
                {
                    order.PaymentStatus = Domain.Enums.PaymentStatus.Success;
                }
                else
                {
                    order.PaymentStatus = Domain.Enums.PaymentStatus.Fail;
                    var warehouse = await _unit.WarehouseDetailRepository.GetByIdAsync(order.WarehouseDetailId);
                    warehouse.Quantity++;
                    _unit.WarehouseDetailRepository.Update(warehouse);
                }
                _unit.OrderRepository.Update(order);
                await _unit.SaveChangeAsync();
            }
            catch (Exception)
            {
                return;
            }

        }


    }
}
