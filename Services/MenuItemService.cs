using AutoMapper;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMapper _mapper;

        public MenuItemService(IMenuItemRepository menuItemRepository, IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MenuItemDTO>> GetAllMenuItemsAsync()
        {
            var menuItems = await _menuItemRepository.GetMenuItemsWithCategoryAsync();
            return _mapper.Map<IEnumerable<MenuItemDTO>>(menuItems);
        }

        public async Task<MenuItemDTO> GetMenuItemByIdAsync(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            return _mapper.Map<MenuItemDTO>(menuItem);
        }

        public async Task<MenuItemDTO> CreateMenuItemAsync(MenuItemDTO menuItemDto)
        {
            var menuItem = _mapper.Map<MenuItem>(menuItemDto);
            menuItem.CreatedAt = System.DateTime.UtcNow;
            await _menuItemRepository.AddAsync(menuItem);
            await _menuItemRepository.SaveChangesAsync();
            return _mapper.Map<MenuItemDTO>(menuItem);
        }

        public async Task<bool> UpdateMenuItemAsync(int id, MenuItemDTO menuItemDto)
        {
            var existingMenuItem = await _menuItemRepository.GetByIdAsync(id);
            if (existingMenuItem == null)
            {
                return false;
            }
            _mapper.Map(menuItemDto, existingMenuItem);
            _menuItemRepository.Update(existingMenuItem);
            await _menuItemRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null)
            {
                return false;
            }
            _menuItemRepository.Delete(menuItem);
            await _menuItemRepository.SaveChangesAsync();
            return true;
        }

        
    }
}