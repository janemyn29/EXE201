using Application.Interfaces;
using Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAPI.Services;

namespace WebAPI.Areas.Admin.Controllers
{
    [Route("Admin/api/[controller]")]
    [ApiController]
    public class ServicePaymentController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IClaimsService _claims;
        private readonly MonthService _service;

        public ServicePaymentController(AppDbContext context, IClaimsService claims, MonthService service)
        {
            _context = context;
            _claims = claims;
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var payments = await _context.ServicePayment.Include(x => x.Contract).ThenInclude(x=>x.Customer).Where(x => !x.IsDeleted).ToListAsync();
            foreach (var item in payments)
            {
                item.Contract.ServicePayments = null;
                item.Contract.Customer.Contracts = null;
            }
            return Ok(payments);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var payments = await _context.ServicePayment.Include(x => x.Contract).Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (payments == null)
            {
                return BadRequest("Không tìm thấy hóa đơn hàng tháng mà bạn yêu cầu");
            }
            payments.Contract.ServicePayments = null;

            return Ok(payments);
        }
    }
}
