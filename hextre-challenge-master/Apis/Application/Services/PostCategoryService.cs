using Application.Interfaces;
using Application.ViewModels.PostCategoryViewModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PostCategoryService : IPostCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IMapper _mapper;

        public PostCategoryService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IMapper mapper)
        { 
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
        }
        public async Task<List<PostCategoryViewModel>> GetPostCategory()
        {
            var post = await _unitOfWork.PostCategoryRepository.GetAllAsync();
            var mapper = _mapper.Map<List<PostCategoryViewModel>>(post);
            return mapper;
        }

        public async Task<bool> CreatePostCategory(CreatePostCategoryViewModel createPostCategoryViewModel)
        {
            var mapper = _mapper.Map<PostCategory>(createPostCategoryViewModel);

            await _unitOfWork.PostCategoryRepository.AddAsync(mapper);

            if (await _unitOfWork.SaveChangeAsync() > 0)
                return true;
            else throw new Exception("Add PostCategory faild.");
        }

        public async Task<bool> UpdatePostCategory(UpdatePostCategoryViewModel updatePostCategoryViewModel)
        {
            var mapper = _mapper.Map<PostCategory>(updatePostCategoryViewModel);
            _unitOfWork.PostCategoryRepository.Update(mapper);

            if (await _unitOfWork.SaveChangeAsync() > 0)
                return true;
            else throw new Exception("Update PostCategory faild.");
        }

        public async Task<bool> DeletePostCategory(Guid id)
        {
            var result = await _unitOfWork.PostCategoryRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Don't found this PostCategory");
            else
            {
                result.IsDeleted = true;
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
        }
    }
}
