using AutoMapper;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;

namespace RestaurantManagementSystem.Services.Impl
{
    public class CategoryServiceImpl : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryServiceImpl(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponse>> GetCategories()
        {
            var categories = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<CategoryResponse> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }
            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryDTO categoryDto)
        {
            // Kiểm tra nghiệp vụ: Tên danh mục không được trùng
            var existingCategory = await _repository.GetByNameAsync(categoryDto.Name);
            if (existingCategory != null)
            {
                // Ném exception này sẽ được filter chuyển thành 409 Conflict
                throw new InvalidOperationException($"A category with the name '{categoryDto.Name}' already exists.");
            }

            var category = _mapper.Map<Category>(categoryDto);
            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();
            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse> UpdateAsync(int id, CreateCategoryDTO request)
        {
            var existingCategory = await _repository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException("Category is not found");
            }

            _mapper.Map(request, existingCategory); // Map changes from DTO into the existing entity
            _repository.Update(existingCategory);
            await _repository.SaveChangesAsync();


            return _mapper.Map<CategoryResponse>(existingCategory);
        }

        public async Task DeleteAsync(int id)
        {
            //// Kiểm tra nghiệp vụ: Không được xóa danh mục nếu còn món ăn
            //var categoryToDelete = await _categoryRepository.GetByIdWithMenuItemsAsync(id);
            //if (categoryToDelete == null)
            //{
            //    // Ném exception này sẽ được filter chuyển thành 404 Not Found
            //    throw new KeyNotFoundException($"Category with ID {id} not found.");
            //}

            //if (categoryToDelete.MenuItems != null && categoryToDelete.MenuItems.Any())
            //{
            //    // Ném exception này sẽ được filter chuyển thành 409 Conflict
            //    throw new InvalidOperationException("Cannot delete a category that has associated menu items.");
            //}
            var category = await _repository.GetByIdAsync(id);
            _repository.Delete(category);
            await _repository.SaveChangesAsync();
        }

        
    }
}
