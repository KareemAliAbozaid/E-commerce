﻿using AutoMapper;
using E_commerce.Web.Core.Models;
using E_commerce.Web.Core.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_commerce.Web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Categories
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryFormViewModel, Category>().ReverseMap();
            CreateMap<Category, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
            //Products
            CreateMap<ProdectFormViewModel, Product>().ReverseMap();
            CreateMap<ProductViewModel, Product>().ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category!.Name));
        }
    }
}
