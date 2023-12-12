using Application;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.GoodViewModels;
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

        [HttpPost("CreateRequestWithGoods")]
        public async Task<IActionResult> CreateRequestWithGoods(RequestWithGoodViewModel requestWithGoodViewModel)
        {
            var myTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var requestAdd = _mapper.Map<Request>(requestWithGoodViewModel.CreateRequestWithRequestDetailViewModel);
                requestAdd.StaffId = requestWithGoodViewModel.CreateRequestWithRequestDetailViewModel.StaffId;
                requestAdd.CustomerId = requestWithGoodViewModel.CreateRequestWithRequestDetailViewModel.CustomerId;
                requestAdd.CompleteDate = DateTime.Now;
                requestAdd.DenyReason = requestWithGoodViewModel.CreateRequestWithRequestDetailViewModel.DenyReason;
                requestAdd.RequestStatus = RequestStatus.Pending;
                requestAdd.RequestType = RequestType.Store;

                foreach (var goodCreateWithImage in requestWithGoodViewModel.GoodCreateWithImage)
                {
                    var result = _mapper.Map<Good>(goodCreateWithImage.GoodCreateModel);
                    result.GoodCategoryId = Guid.Parse("3963bb64-cd45-4300-9554-4555d55e5054");                 
                    result.ExpirationDate = DateTime.Now;

                    await _context.Good.AddAsync(result);
                    await _context.SaveChangesAsync();

                    List<GoodImage> images = new List<GoodImage>();
                    foreach (var item in goodCreateWithImage.Images)
                    {
                        var goodImage = new GoodImage();
                        goodImage.GoodId = result.Id;
                        goodImage.ImageUrl = item;
                        images.Add(goodImage);
                    }

                    await _context.GoodImage.AddRangeAsync(images);
                    await _context.SaveChangesAsync();



                    foreach (var requestDetail in requestWithGoodViewModel.CreateRequestWithRequestDetailViewModel.RequestDetails)
                    {
                        requestDetail.RequestId = requestAdd.Id;
                        requestDetail.GoodId = result.Id;
                        requestDetail.Quantity = result.Quantity;
                        
                    }
                }
              
                await _context.Request.AddAsync(requestAdd);
                await _context.SaveChangesAsync();
                await myTransaction.CommitAsync();

                return Ok("Tạo yêu cầu thêm hàng vào kho thành công!");
            }
            catch (Exception ex)
            {
                await myTransaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("UpdateStatusAddGoods")]
        public async Task<IActionResult> UpdateStatusAddGoods(Guid id, RequestStatus requestStatus, string? denyReason)
        {
            try
            {
                var req = await _context.Request.Where(x => x.Id == id && !x.IsDeleted).AsNoTracking().FirstOrDefaultAsync();
                if (req == null)
                {
                    return BadRequest("Không tìm thấy yêu cầu!");
                }
                else
                {
                    if (requestStatus == RequestStatus.Complete)
                    {
                        req.CompleteDate = DateTime.Now;
                        req.RequestStatus = RequestStatus.Complete;
                        var details = await _context.RequestDetail.Include(x => x.Good).AsNoTracking().Where(x => x.RequestId == req.Id).ToListAsync();
                        foreach (var item in details)
                        {
                            item.Good.IsDeleted = false;
                            await _context.SaveChangesAsync();
                        }
                    }
                    else if (requestStatus == RequestStatus.Canceled)
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
    }
}
