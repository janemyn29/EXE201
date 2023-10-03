using Application.Interfaces;
using Application.Services;
using Application.ViewModels.PostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Pkcs;

namespace WebAPI.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ("Admin"))]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        { 
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPost()
        {
            var result = await _postService.GetPost();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreatePostViewModel createPostViewModel)
        {
            try
            {
                var result = await _postService.CreatePost(createPostViewModel);
                return Ok(new
                {
                    Result = "Create successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost(UpdatePostViewModel updatePostViewModel)
        {
            try
            {
                var result = await _postService.UpdatePost(updatePostViewModel);
                return Ok(new
                {
                    Reuslt = "Update successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            try
            {
                var result = await _postService.DeletePost(id);
                return Ok(new
                {
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
