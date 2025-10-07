using AutoMapper;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Services.Impl
{
    public class MenuItemServiceImpl : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly ICategoryRepository _categoryRepository; // Cần để kiểm tra Category tồn tại
        private readonly IMapper _mapper;

        public MenuItemServiceImpl(IMenuItemRepository menuItemRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MenuItemResponse>> GetAllMenuItemsAsync()
        {
            var menuItems = await _menuItemRepository.GetAllWithCategoryAsync();
            // Map từ List<Model> sang List<Response> và trả về
            return _mapper.Map<IEnumerable<MenuItemResponse>>(menuItems);
        }

        public async Task<MenuItemResponse> GetMenuItemByIdAsync(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdWithCategoryAsync(id);
            if (menuItem == null)
            {
                throw new KeyNotFoundException($"Menu item with ID {id} not found.");
            }
            // Map từ Model sang Response và trả về
            return _mapper.Map<MenuItemResponse>(menuItem);
        }

        public async Task<MenuItemResponse> CreateMenuItemAsync(MenuItemCreateDTO menuItemDto)
        {
            // Kiểm tra nghiệp vụ: CategoryId phải tồn tại
            var categoryExists = await _categoryRepository.GetByIdAsync(menuItemDto.CategoryId);
            if (categoryExists == null)
            {
                // Ném lỗi này sẽ được filter chuyển thành 400 Bad Request
                throw new ArgumentException($"Category with ID {menuItemDto.CategoryId} does not exist.");
            }

            // Kiểm tra nghiệp vụ: Tên món ăn không được trùng
            var existingItem = await _menuItemRepository.GetByNameAsync(menuItemDto.ProductName);
            if (existingItem != null)
            {
                // Ném lỗi này sẽ được filter chuyển thành 409 Conflict
                throw new InvalidOperationException($"A menu item with the name '{menuItemDto.ProductName}' already exists.");
            }

            var menuItem = _mapper.Map<MenuItem>(menuItemDto);
            menuItem.CreatedAt = DateTime.UtcNow;

            await _menuItemRepository.AddAsync(menuItem);
            await _menuItemRepository.SaveChangesAsync();

            // Lấy lại dữ liệu kèm Category để map response cho đầy đủ
            var createdItemWithDetails = await _menuItemRepository.GetByIdWithCategoryAsync(menuItem.Id);
            return _mapper.Map<MenuItemResponse>(createdItemWithDetails);
        }

        public async Task<MenuItemResponse> UpdateMenuItemAsync(int id, MenuItemUpdateDTO menuItemDto)
        {
            var existingItem = await _menuItemRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                throw new KeyNotFoundException($"Menu item with ID {id} not found.");
            }

            _mapper.Map(menuItemDto, existingItem);
            _menuItemRepository.Update(existingItem);
            await _menuItemRepository.SaveChangesAsync();
            return _mapper.Map<MenuItemResponse>(existingItem);

        }

        public async Task DeleteMenuItemAsync(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null)
            {
                throw new KeyNotFoundException($"Menu item with ID {id} not found.");
            }
            // Logic kiểm tra xem món ăn có trong order nào không nên được thêm ở đây
            //_menuItemRepository.Delete(menuItem);
            //await _menuItemRepository.SaveChangesAsync();
            menuItem.IsAvailable = false; // Chuyển trạng thái thành không còn phục vụ
            _menuItemRepository.Update(menuItem);
        }

    }
}