using InventoryManagement.Application.Interfaces;
using InventoryManagement.Application.ViewModels;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.Services
{
    public class CategoryApplication : ICategoryApplication
    {
        #region Dependency Injection
        private readonly ICategoryRepository _categoryRepository;
        public CategoryApplication(ICategoryRepository ICategoryRepository)
        {
            _categoryRepository = ICategoryRepository;
        }
        #endregion

        public async Task<IEnumerable<vmCategory>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(x => new vmCategory
            {
                CategoryId = x.CategoryId,
                Name = x.Name,
                Description = x.Description
            });
        }

        public async Task<vmCategory?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return new vmCategory
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };
        }

        public async Task AddAsync(vmCategory category)
        {
            var entity = new Category
            {
                Name = category.Name,
                Description = category.Description
            };

            await _categoryRepository.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(int categoryId, vmCategory category)
        {
            var getCategory = await _categoryRepository.GetByIdAsync(categoryId);
            if (getCategory == null) return false;

            getCategory.Name = category.Name;
            getCategory.Description = category.Description;
            await _categoryRepository.UpdateAsync(getCategory);
            return true;
        }

        public async Task<bool> DeleteAsync(int categoryId)
        {  
            await _categoryRepository.DeleteAsync(categoryId);
            return true;
        }
    }
}
