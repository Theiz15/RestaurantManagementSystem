using AutoMapper;
using RestaurantManagementSystem.DTOs;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Models;
using System.Linq;

namespace RestaurantManagementSystem.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserCreateDTO, User>();
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));
            CreateMap<UserUpdateDTO, User>();

            // Từ Entity sang DTO
            CreateMap<Category, CategoryResponse>();
            // Từ DTO sang Entity (dùng cho cả Create và Update)
            CreateMap<CreateCategoryDTO, Category>();

            // Thêm các mapping cho MenuItem
            CreateMap<MenuItemCreateDTO, MenuItem>();
            CreateMap<MenuItemUpdateDTO, MenuItem>();
            CreateMap<MenuItem, MenuItemResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "N/A"));

            CreateMap<FileUpload, FileUploadResponse>()
            .ForMember(dest => dest.OriginalName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.FileType, opt => opt.MapFrom(src => src.FileType.ToString())) // Chuyển enum thành string
            .ForMember(dest => dest.Url, opt => opt.Ignore()); // Sẽ được tạo trong service
        }
    }
}