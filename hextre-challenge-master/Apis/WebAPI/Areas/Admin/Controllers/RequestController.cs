using Application;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.PostViewModels;
using Application.ViewModels.RequestDetailViewModel;
using Application.ViewModels.RequestViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
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
        public async Task<IActionResult> Get()
        {
            var list = await _context.Request.Include(x => x.Details).Include(x => x.Customer).Where(x => x.IsDeleted == false).ToListAsync();
            var result = _mapper.Map<List<RequestModel>>(list);
            return Ok(result);
        }


        [HttpPost]
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

        [HttpPut]
        public async Task<IActionResult> UpdateRequest(UpdateRequestViewModel updateRequestViewModel)
        {
            try
            {
                var req = await _context.Request.Where(x => x.Id == updateRequestViewModel.Id && !x.IsDeleted).AsNoTracking().FirstOrDefaultAsync();
                if (req == null)
                {
                    return BadRequest("Không tìm thấy yêu cầu!");
                }
                else
                {
                    if (updateRequestViewModel.RequestStatus == RequestStatus.Complete)
                    {
                        req.CompleteDate = DateTime.Now;
                        req.RequestStatus = RequestStatus.Complete;
                        var details = await _context.RequestDetail.AsNoTracking().Where(x => x.RequestId == req.Id && !x.IsDeleted).ToListAsync();
                        foreach (var item in details)
                        {
                            var good = await _context.Good.AsNoTracking().FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == item.GoodId);
                            if (req.RequestType == RequestType.PickUp)
                            {
                                good.Quantity = good.Quantity - item.Quantity;
                            }
                            else
                            {
                                good.Quantity = good.Quantity + item.Quantity;
                            }

                            _context.Good.Update(good);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else if (updateRequestViewModel.RequestStatus == RequestStatus.Canceled)
                    {
                        req.DenyReason = "Yêu cầu đã bị nhân viên từ chối!";
                        req.RequestStatus = RequestStatus.Canceled;
                    }
                    _context.Request.Update(req);
                    await _context.SaveChangesAsync();
                    return Ok("Cập nhật trạng thái yêu cầu thành công!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(Guid id, RequestStatus requestStatus, string? denyReason)
        {
            try
            {
               var req = await _context.Request.Where(x=>x.Id == id && !x.IsDeleted).AsNoTracking().FirstOrDefaultAsync();
                if(req == null)
                {
                    return BadRequest("Không tìm thấy yêu cầu!");
                }
                else
                {
                    if(requestStatus == RequestStatus.Complete)
                    {
                        req.CompleteDate = DateTime.Now;
                        req.RequestStatus = RequestStatus.Complete;
                        var details = await _context.RequestDetail.AsNoTracking().Where(x => x.RequestId == req.Id && !x.IsDeleted).ToListAsync();
                        foreach (var item in details)
                        {
                            var good = await _context.Good.AsNoTracking().FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == item.GoodId);
                            if(req.RequestType == RequestType.PickUp)
                            {
                                good.Quantity = good.Quantity - item.Quantity;
                            }
                            else
                            {
                                good.Quantity = good.Quantity + item.Quantity;
                            }
                            
                            _context.Good.Update(good);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else if(requestStatus == RequestStatus.Canceled)
                    {
                        req.DenyReason = denyReason;
                        req.RequestStatus = RequestStatus.Canceled;
                    }
                    _context.Request.Update(req);
                    await _context.SaveChangesAsync();
                    return Ok("Cập nhật trạng thái yêu cầu thành công!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(Guid id)
        {
            try
            {
                var result = await _requestService.DeleteRequest(id);
                return Ok(new
                {
                    Result = "Xoá thành công."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetRequestAdd")]
        public async Task<IActionResult> GetIncluding(Guid id)
        {
            var request = await _context.Request.Include(x => x.Details).ThenInclude(x => x.Good).ThenInclude(x => x.GoodImages).Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == id);
            return Ok(request);
        }

        [HttpPost("AddRequestToWareHouse")]
        public async Task<IActionResult> AddRequest(RequestWithGoodViewModel requestWithGoodViewModel)
        {
            var myTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (requestWithGoodViewModel.GoodCreateWithImage.Images == null || requestWithGoodViewModel.GoodCreateWithImage.Images.Count == 0)
                {
                    return BadRequest("Vui lòng thêm hình ảnh!");
                }
                var result = _mapper.Map<Good>(requestWithGoodViewModel.GoodCreateWithImage.GoodCreateModel);
                result.GoodCategoryId = Guid.Parse("3963bb64-cd45-4300-9554-4555d55e5054");
                result.ExpirationDate = DateTime.Now;
                result.IsDeleted = true;               
                await _context.Good.AddAsync(result);
                await _context.SaveChangesAsync();
                List<GoodImage> images = new List<GoodImage>();
                foreach (var item in requestWithGoodViewModel.GoodCreateWithImage.Images)
                {
                    var goodImage = new GoodImage();
                    goodImage.GoodId = result.Id;
                    goodImage.ImageUrl = item;
                    images.Add(goodImage);
                }
                await _context.GoodImage.AddRangeAsync(images);
                await _context.SaveChangesAsync();
                await myTransaction.CommitAsync();
                requestWithGoodViewModel.CreateRequestViewModel.RequestId = Guid.NewGuid();
                requestWithGoodViewModel.CreateRequestViewModel.StaffId = requestWithGoodViewModel.CreateRequestViewModel.StaffId;
                requestWithGoodViewModel.CreateRequestViewModel.CustomerId = requestWithGoodViewModel.CreateRequestViewModel.CustomerId;
                requestWithGoodViewModel.CreateRequestViewModel.CompleteDate = DateTime.Now;
                requestWithGoodViewModel.CreateRequestViewModel.DenyReason = requestWithGoodViewModel.CreateRequestViewModel.DenyReason;
                requestWithGoodViewModel.CreateRequestViewModel.RequestStatus = RequestStatus.Pending;
                requestWithGoodViewModel.CreateRequestViewModel.RequestType = RequestType.Store;
                if (requestWithGoodViewModel.CreateRequestViewModel.RequestDetails == null)
                {
                    requestWithGoodViewModel.CreateRequestViewModel.RequestDetails = new List<CreateRequestDetailViewModel>();
                }
                foreach (var requestDetail in requestWithGoodViewModel.CreateRequestViewModel.RequestDetails)
                {
                    requestDetail.RequestId = requestWithGoodViewModel.CreateRequestViewModel.RequestId;
                    requestDetail.GoodId = result.Id;
                    requestDetail.Quantity = requestDetail.Quantity;
                    var check = await _context.Good.FirstOrDefaultAsync(x => x.IsDeleted == false && x.GoodName.ToLower().Equals(requestWithGoodViewModel.GoodCreateWithImage.GoodCreateModel.GoodName.ToLower()));
                    if (check != null)
                    {
                        check.Quantity = check.Quantity + requestDetail.Quantity;
                    }
                }
                var requestAdd = _mapper.Map<Request>(requestWithGoodViewModel.CreateRequestViewModel);
                await _context.Request.AddAsync(requestAdd);
                await _context.SaveChangesAsync();
                return Ok("Tạo yêu cần thêm hàng vào kho thành công!");
            }
            catch (Exception ex)
            {
                await myTransaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateRequestStatus/{requestId}")]
        public async Task<IActionResult> UpdateRequestStatus(Guid requestId, [FromBody] UpdateRequestStatusModel updateRequestStatusModel)
        {
            using (var myTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingRequest = await _context.Request
                        .Include(r => r.Details)
                        .ThenInclude(rd => rd.Good)
                        .FirstOrDefaultAsync(r => r.Id == requestId);
                    if (existingRequest == null)
                    {
                        return NotFound("Yêu cầu không tồn tại");
                    }
                    if (updateRequestStatusModel.RequestStatus == RequestStatus.Complete)
                    {
                        existingRequest.RequestStatus = RequestStatus.Complete;
                        foreach (var detail in existingRequest.Details)
                        {
                            detail.Good.IsDeleted = false;
                        }
                        await _context.SaveChangesAsync();
                    }
                    await myTransaction.CommitAsync();
                    return Ok("Cập nhật trạng thái yêu cầu thành công!");
                }
                catch (Exception ex)
                {
                    await myTransaction.RollbackAsync();
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
