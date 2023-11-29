using Application.Interfaces;
using Domain.Entities;
using Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Customer")]
    public class ServicePaymentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IClaimsService _claims;
        private readonly IOrderService _service;

        public ServicePaymentController(AppDbContext context, IClaimsService claims,IOrderService service )
        {
            _context = context;
            _claims = claims;
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = _claims.GetCurrentUserId;
            var payments = await _context.ServicePayment.Include(x=>x.Contract).Where(x=>!x.IsDeleted).ToListAsync();
            var temp = payments.Where(x=>x.Contract.CustomerId.ToLower().Equals(userId.ToString().ToLower())).ToList();

            foreach (var item in payments)
            {
                item.Contract.ServicePayments = null;
            }
            return Ok(temp);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var userId = _claims.GetCurrentUserId;
            var payments = await _context.ServicePayment.Include(x => x.Contract).Where(x => !x.IsDeleted && x.Id ==id && x.Contract.CustomerId.ToLower().Equals(userId)).FirstOrDefaultAsync();
            
            if ( payments == null)
            {
                return BadRequest("Không tìm thấy hóa đơn hàng tháng mà bạn yêu cầu");
            }
            payments.Contract.ServicePayments = null;

            
            return Ok(payments);
        }

        [HttpPost]
        public async Task<IActionResult> Payment(Guid servicePaymentId,  string option = "payWithATM")
        {
            try
            {
                var userId = _claims.GetCurrentUserId;

                var payments = await _context.ServicePayment.Include(x => x.Contract).Where(x => !x.IsDeleted).ToListAsync();
                var temp = payments.Where(x => x.Contract.CustomerId.ToLower().Equals(userId.ToString().ToLower())).ToList();
                var ser = temp.Where(x=>x.Id == servicePaymentId).FirstOrDefault();
                if (ser == null)
                {
                    return BadRequest("Không tìm thấy hóa đơn cần thanh toán");
                }else if(ser.IsPaid)
                {
                    return BadRequest("Hóa đơn này đã được thanh toán");
                }
                var tempcheck = await _service.PaymentMonly(ser, option);
                return Ok(tempcheck);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
