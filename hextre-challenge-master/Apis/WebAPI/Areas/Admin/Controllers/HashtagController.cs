using Application.Interfaces;
using Application.ViewModels.HashtagViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]
    public class HashtagController : ControllerBase
    {
        private readonly IHashtagService _hashtagService;

        public HashtagController(IHashtagService hashtagService)
        {
            _hashtagService = hashtagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHashtag()
        {
            var result = await _hashtagService.GetHashtag();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostHashtag(CreateHashtagViewModel createHashtagView)
        {
            try
            {
                var result = await _hashtagService.CreateHashtag(createHashtagView);
                return Ok(new {
                    Result = "Create successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateHashtag(UpdateHashtagViewModel updateHashtagView)
        {
            try
            {
                var result = _hashtagService.UpdateHashtag(updateHashtagView);
                return Ok(new
                {
                    Result = "Update successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHashtag(Guid id)
        {
            try
            {
                var result = await _hashtagService.DeleteHashtag(id);

                return Ok(new { 
                    Reuslt = "Delete successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
