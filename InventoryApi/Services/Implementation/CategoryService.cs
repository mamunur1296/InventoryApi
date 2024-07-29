using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;
using System.Data.Common;

namespace InventoryApi.Services.Implementation
{
    public class CategoryService : IBaseServices<CategoryDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public CategoryService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<ResponseDTOs<string>> CreateAsync(CategoryDTOs entity)
        {
            var response = new ResponseDTOs<string>();
            var newCategory = new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = entity.Name,
                //Products= entity.Products,

            };
            await _unitOfWorkRepository.categoryRepository.AddAsync(newCategory);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<string>> DeleteAsync(string id)
        {
            var response = new ResponseDTOs<string>();
            var deleteItem = await _unitOfWorkRepository.categoryRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Catagory with id = {id} not found");
            }
            await _unitOfWorkRepository.categoryRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<IEnumerable<CategoryDTOs>>> GetAllAsync()
        {
            var response = new ResponseDTOs<IEnumerable<CategoryDTOs>>();


            var CatagoryList = await _unitOfWorkRepository.categoryRepository.GetAllAsync();
            var result = CatagoryList.Select(x => new CategoryDTOs()
            {
                id = x.Id,
               Name = x.Name,
               //Products=x.Products,
            });
            response.Data = result;
            response.Success = true;
            return response;
        }

        public Task<ResponseDTOs<CategoryDTOs>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTOs<string>> UpdateAsync(CategoryDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
