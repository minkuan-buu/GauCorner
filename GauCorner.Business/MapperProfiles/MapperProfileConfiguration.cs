﻿using AutoMapper;
using GauCorner.Business.Utilities.Authentication;
using GauCorner.Business.Utilities.Converter;
using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.ResponseModel;
using GauCorner.Data.DTO.ResponseModel.UserResModel;
using GauCorner.Data.Entities;
using Microsoft.Data.SqlClient;

namespace GauCorner.Business.MapperProfiles
{
    public class MapperProfileConfiguration : Profile
    {
        public MapperProfileConfiguration()
        {
            //Login
            CreateMap<UserAccount, UserLoginResModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => TextConvert.ConvertFromUnicodeEscape(src.Name)))
                .ForMember(dest => dest.UserPath, opt => opt.MapFrom(src => src.Path))
                .ForPath(dest => dest.Auth.DeviceId, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForPath(dest => dest.Auth.RefreshToken, opt => opt.MapFrom(src => Authentication.GenerateRefreshToken()))
                .ForPath(dest => dest.Auth.Token, opt => opt.MapFrom(src => Authentication.GenerateJWT(src)));

            CreateMap<UserRegisterModel, UserAccount>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => TextConvert.ConvertToUnicodeEscape(src.Name)))
                .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<DonateReqModel, Donate>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => TextConvert.ConvertToUnicodeEscape(src.Message)))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => TextConvert.ConvertToUnicodeEscape(src.Username)));

            CreateMap<UserAccount, DonatePageResModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => TextConvert.ConvertFromUnicodeEscape(src.Name)))
                .ForMember(dest => dest.UserPath, opt => opt.MapFrom(src => src.Path))
                .ForMember(dest => dest.BackgroundUrl, opt => opt.MapFrom(src => src.Uiconfigs.FirstOrDefault(x => x.IsUsed).BackgroundUrl))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.Uiconfigs.FirstOrDefault(x => x.IsUsed).LogoUrl))
                .ForMember(dest => dest.ColorTone, opt => opt.MapFrom(src => src.Uiconfigs.FirstOrDefault(x => x.IsUsed).ColorTone))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => TextConvert.ConvertFromUnicodeEscape(src.Uiconfigs.FirstOrDefault(x => x.IsUsed).Description)));

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => TextConvert.ConvertToUnicodeEscape(src.Name)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => TextConvert.ConvertToUnicodeEscape(src.Description)))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<CategoryReqModel, Category>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    TextConvert.ConvertToUnicodeEscape(src.CategoryName)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true));

            CreateMap<SubCategoryReqModel, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                TextConvert.ConvertToUnicodeEscape(src.CategoryName)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true));
            CreateMap<Category, SubCategoryResModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    TextConvert.ConvertFromUnicodeEscape(src.Name)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<Category, CategoryResModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    TextConvert.ConvertFromUnicodeEscape(src.Name)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<Uiconfig, DonatePageConfigLabel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => TextConvert.ConvertFromUnicodeEscape(src.Name)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IsUsed, opt => opt.MapFrom(src => src.IsUsed));

            CreateMap<Uiconfig, DonatePageConfigResModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => TextConvert.ConvertFromUnicodeEscape(src.Name)))
                .ForMember(dest => dest.BackgroundUrl, opt => opt.MapFrom(src => src.BackgroundUrl))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.LogoUrl))
                .ForMember(dest => dest.ColorTone, opt => opt.MapFrom(src => src.ColorTone))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => TextConvert.ConvertFromUnicodeEscape(src.Description)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<ConfigDto, Uiconfig>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => TextConvert.ConvertToUnicodeEscape(src.Name)))
                .ForMember(dest => dest.BackgroundUrl, opt => opt.MapFrom(src => src.BackgroundImage))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.LogoImage))
                .ForMember(dest => dest.ColorTone, opt => opt.MapFrom(src => src.ColorTone))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => TextConvert.ConvertToUnicodeEscape(src.Description)));

            CreateMap<StreamConfig, StreamConfigResModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.StreamConfigTypeId))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => TextConvert.ConvertFromUnicodeEscape(src.StreamConfigType.Name)))
                .ForMember(dest => dest.AlternativeName, opt => opt.MapFrom(src => TextConvert.ConvertFromUnicodeEscape(src.StreamConfigType.AlternativeName)))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => TextConvert.ConvertFromUnicodeEscape(src.Value)));
        }
    }
}