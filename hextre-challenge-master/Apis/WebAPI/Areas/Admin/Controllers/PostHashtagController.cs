﻿using Application.Interfaces;
using Application.ViewModels.PostHashtagViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Org.BouncyCastle.Crypto.Paddings;
using System.Data;
using System.Security.Policy;

namespace WebAPI.Areas.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = ("Admin"))]
    public class PostHashtagController : ControllerBase
    {
        private readonly IPostHashtagService _postHashtagService;

        public PostHashtagController(IPostHashtagService postHashtagService)
        {
            _postHashtagService = postHashtagService;
        }

        [HttpGet("ListPostAndHashtag")]
        public async Task<IActionResult> GetPostAndHashtag()
        {
            return Ok(await _postHashtagService.GetPostAndHashtag());
        }

        [HttpGet]
        public async Task<IActionResult> GetPostHashtag()
        {
            return Ok(await _postHashtagService.GetPostHashtag());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePostHashtag(CreatePostHashtagViewModel createPostHashtagView)
        {
            try
            {
                await _postHashtagService.CreatePostHashtag(createPostHashtagView);
                return Ok("Create successfully.");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePostHashtag(CreatePostHashtagViewModel hashtagViewModel)
        {
            try
            {
                await _postHashtagService.UpdatePostHashtag(hashtagViewModel);
                return Ok("Update successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePostHashtag(Guid id)
        {
            try
            {
                await _postHashtagService.DeletePostHashtag(id);
                return Ok("Delete successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
