﻿using Application.ViewModels.ChemicalsViewModels;
using AutoMapper;
using Application.Commons;
using Domain.Entities;
using Application.ViewModels.CategoryViewModels;
using Application.ViewModels.ProviderViewModels;
using Application.ViewModels.WarehouseViewModel;
using Application.ViewModels.ImageWarehouseViewModels;
using Application.ViewModels.WarehouseDetailViewModels;
using Application.ViewModels.OrderViewModels;
using Application.ViewModels.PostCategoryViewModels;
using Application.ViewModels.HashtagViewModels;
using Application.ViewModels.PostViewModels;
using Application.ViewModels.AuthViewModel;

namespace Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<CreateChemicalViewModel, Chemical>();
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<Chemical, ChemicalViewModel>()
                .ForMember(dest => dest._Id, src => src.MapFrom(x => x.Id));
            CreateMap<CreateCategoryViewModel, Category>().ReverseMap();
            CreateMap<UpdateCategoryViewModel, Category>().ReverseMap();
            CreateMap<CategoryViewModel, Category>().ReverseMap();

            CreateMap<CreateProviderViewModel, Provider>().ReverseMap();
            CreateMap<UpdateProviderViewModel, Provider>().ReverseMap();

            CreateMap<Provider, ProviderViewModel>().ForMember(des => des.WarehousesCount, src => src.MapFrom(x => x.Warehouses.Count))
                .ReverseMap();

            CreateMap<WarehoureUpdateModel, Warehouse>().ReverseMap();
            CreateMap<WarehourseCreateModel, Warehouse>().ReverseMap();
            CreateMap<Warehouse, WarehouseViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.ProviderName, opt => opt.MapFrom(src => src.Provider.Name))
                .ForMember(dest => dest.imageURL, opt => opt.MapFrom(src => src.ImageWarehouses.FirstOrDefault().ImageURL));

            CreateMap<WarehouseDetailViewModel, WarehouseDetail>().ReverseMap();
            CreateMap<WarehouseDetailCreateModel, WarehouseDetail>().ReverseMap();
            CreateMap<WarehouseDetail, WarehouseDetailUpdateModel>().ReverseMap();

            CreateMap<ImageWarehouseViewModel, ImageWarehouse>().ReverseMap();
            CreateMap<ImageWarehouseCreateModel, ImageWarehouse>().ReverseMap();


            CreateMap<OrderViewModel, Order>().ReverseMap();
            /*CreateMap<ImageWarehouseCreateModel, ImageWarehouse>().ReverseMap();*/

            CreateMap<PostCategoryViewModel, PostCategory>().ReverseMap();
            CreateMap<CreatePostCategoryViewModel, PostCategory>().ReverseMap();
            CreateMap<UpdatePostCategoryViewModel, PostCategory>().ReverseMap();

            CreateMap<CreateHashtagViewModel, Hashtag>().ReverseMap();
            CreateMap<UpdateHashtagViewModel, Hashtag>().ReverseMap();
            CreateMap<HashtagViewModel, Hashtag>().ReverseMap();

            CreateMap<CreatePostViewModel, Post>().ReverseMap();
            CreateMap<Post, PostViewModel>()
                .ForMember(des => des.NamePostCategory, src => src.MapFrom(x => x.PostCategory.Name))
                .ForMember(des => des.FullnameAuthor, src => src.MapFrom(x => x.Author.Fullname))
                .ReverseMap();
            CreateMap<UpdatePostViewModel, Post>().ReverseMap();

            CreateMap<ApplicationUser, UpdateAuthViewModel>().ReverseMap();
        }
    }
}
