using System;
using System.Linq;
using AutoMapper;
using API.Entities;
using API.DTOs;
using API.Extensions;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace API.Helpers
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<AppUser, MemberDto>()
				.ForMember(
					dest => dest.PhotoUrl, opt => opt.MapFrom(
						src => src.Photos.FirstOrDefault(
							x => x.IsMain
						).Url
					)
				)
				.ForMember(
					dest => dest.Age, opt => opt.MapFrom (
						src => src.DateOfBirth.CalculateAge()
					)
				);
			CreateMap<Photo, PhotoDto>();
			CreateMap<MemberUpdateDto, AppUser>();
			CreateMap<RegisterDto, AppUser>();

		}
	}
}
