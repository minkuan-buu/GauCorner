using AutoMapper;
using GauCorne.Data.DTO.RequestModel;
using GauCorne.Data.DTO.ResponseModel.UserResModel;
using GauCorner.Business.Utilities.Authentication;
using GauCorner.Business.Utilities.Converter;
using GauCorner.Data.DTO.ResponseModel;
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
                .ForPath(dest => dest.DeviceId, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForPath(dest => dest.RefreshToken, opt => opt.MapFrom(src => Authentication.GenerateRefreshToken()))
                .ForPath(dest => dest.AccessToken, opt => opt.MapFrom(src => Authentication.GenerateJWT(src)));

            CreateMap<UserRegisterModel, UserAccount>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => TextConvert.ConvertToUnicodeEscape(src.Name)))
                .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}