using Application;
using AutoMapper;
using Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("Admin/api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;

        public TransactionController(AppDbContext context, IMapper mapper, IUnitOfWork unit)
        {
            _context = context;
            _mapper = mapper;
            _unit = unit;
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var orders = await _context.Transaction.Include(x=>x.ServicePayment).ThenInclude(x=>x.Contract.Customer).AsNoTracking().Where(x => x.IsDeleted == false).OrderBy(x => x.CreationDate).ToListAsync();
            foreach (var item in orders)
            {
                item.ServicePayment.Contract.ServicePayments = null;
                item.ServicePayment.Contract.Customer.Contracts = null;
            }
            return Ok(orders);
        }
    }
}
