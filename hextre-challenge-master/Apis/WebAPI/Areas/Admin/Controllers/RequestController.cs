using Application.Interfaces;
using Application.ViewModels.PostViewModels;
using Application.ViewModels.RequestViewModels;
using AutoMapper;
using Domain.Entities;
using Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WebAPI.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRequestService _requestService;


        public RequestController(AppDbContext context,IMapper mapper, UserManager<ApplicationUser> userManager, IRequestService requestService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _requestService = requestService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(Guid rentWarehouse)
        {
            var list = await _context.Request.Include(x=>x.Details).ThenInclude(x=>x.Good).ThenInclude(x=>x.RentWarehouse)
                .Where(x=>x.IsDeleted == false && x.Details.FirstOrDefault().Good.RentWarehouseId == rentWarehouse).ToListAsync();
            var warehouse = await _context.RentWarehouse.FirstOrDefaultAsync(x=>x.IsDeleted == false && x.Id == rentWarehouse);
            var staff = await _userManager.FindByIdAsync(warehouse.StaffId);
            var result = _mapper.Map<List<RequestModel>>(list);
            foreach (var item in result)
            {
                foreach (var item1 in item.Details)
                {
                    item1.Request = null;
                    item1.Good.RentWarehouse = null;
                }
                item.RentWarehouseInfor = warehouse.Information;
                item.StaffPhone = staff.PhoneNumber;
                item.StaffEmail = staff.Email;
                item.StaffName = staff.Fullname;
            }
            return Ok(result);
        }

        [HttpPost]
/*        [Authorize(Roles = "Admin,Staff")]*/
        public async Task<IActionResult> CreateRequest(CreateRequestViewModel createRequestViewModel)
        {
            try
            {
                var result = await _requestService.CreateRequest(createRequestViewModel);
                return Ok(new
                {
                    Result = "Tạo thành công."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
