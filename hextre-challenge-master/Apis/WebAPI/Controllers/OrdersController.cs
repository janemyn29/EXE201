using Application;
using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.OrderViewModels;
using AutoMapper;
using Domain.Entities;
using Infrastructures;
using Infrastructures.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public OrdersController(IUnitOfWork unit, IOrderService orderService,IClaimsService claimsService,AppDbContext context,IMapper mapper, INoteRepository noteRepository)
        {
            _unit = unit;
            _orderService = orderService;
            _claimsService = claimsService;
            _context = context;
            _mapper = mapper;
            _noteRepository = noteRepository;
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
                await _orderService.CreateOrder(ordermodel);
                var note = new Notification();
                note.ApplicationUserId = ordermodel.CustomerId;
                note.IsRead = false;
                note.Title = "Đơn hàng mới";
                note.Description = "Bạn vừa tạo đơn hàng thành công. Xin vui lòng thanh toán!";
                await _unit.NoteRepository.AddAsync(note);
                await _unit.SaveChangeAsync();
                return Ok("Tạo đơn hàng công!");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
