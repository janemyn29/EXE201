using Application.ViewModels.OrderViewModels;
using Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DasboardController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public DasboardController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("TotalRevenue")]
        public async Task<IActionResult> TotalRevenue()
        {
            var temp = await _dbContext.Order.Where(x=>!x.IsDeleted && x.PaymentStatus == Domain.Enums.PaymentStatus.Success).SumAsync(x=>x.TotalPrice);
            return Ok(temp);
        }

        [HttpGet("OrderStatic")]
        public async Task<IActionResult> OrderStatic()
        {
            var temp = await _dbContext.Order.Where(x => !x.IsDeleted ).ToListAsync();
            var order = new OrderStatic();
            order.totalOrder = temp.Count();
            order.SuccessPaymentOrder = temp.Where(x => x.PaymentStatus == Domain.Enums.PaymentStatus.Success).Count();
            order.FailPaymentOrder = temp.Where(x => x.PaymentStatus == Domain.Enums.PaymentStatus.Fail).Count();
            order.PendingOrder = temp.Where(x => x.OrderStatus == Domain.Enums.OrderStatus.Pending).Count();
            order.TotalRevenue = temp.Where(x => x.PaymentStatus == Domain.Enums.PaymentStatus.Success).Sum(x=>x.TotalPrice);
            order.ServiceRevenue = temp.Where(x => x.PaymentStatus == Domain.Enums.PaymentStatus.Success).Sum(x => x.ServicePrice);
            order.WarehouseRevenue = temp.Where(x => x.PaymentStatus == Domain.Enums.PaymentStatus.Success).Sum(x => x.WarehousePrice);
            return Ok(order);
        }
    }
}
