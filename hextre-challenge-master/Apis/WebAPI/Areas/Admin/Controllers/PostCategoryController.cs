using Application.Interfaces;
using Application.ViewModels.PostCategoryViewModels;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]
    public class PostCategoryController : ControllerBase
    {
        private readonly IPostCategoryService _postCategoryService;

        public PostCategoryController(IPostCategoryService postCategoryService)
        {
            _postCategoryService = postCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPostCategory()
        {
            var result = await _postCategoryService.GetPostCategory();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostCategory(CreatePostCategoryViewModel createPostCategoryViewModel)
        {
            try
            {
                var result = await _postCategoryService.CreatePostCategory(createPostCategoryViewModel);
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
        public async Task<IActionResult> UpdatePostCategory(UpdatePostCategoryViewModel updatePostCategoryViewModel)
        {
            try
            {
                var result = await _postCategoryService.UpdatePostCategory(updatePostCategoryViewModel);
                return Ok(new { 
                    Reuslt = "Update successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostCategory(Guid id)
        {
            try
            {
                var result = await _postCategoryService.DeletePostCategory(id);
                return Ok(new { 
                    Result = "Delete successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
