using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class ProductService : IBaseServices<ProductDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ProductService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<ResponseDTOs<string>> CreateAsync(ProductDTOs entity)
        {
            var response = new ResponseDTOs<string>();
            var newProduct = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = entity.Name,
                CategoryId = entity.CategoryId,

            };
            await _unitOfWorkRepository.productRepository.AddAsync(newProduct);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<string>> DeleteAsync(string id)
        {
            var response = new ResponseDTOs<string>();
            var deleteItem = await _unitOfWorkRepository.productRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($" Product   with id = {id} not found");
            }
            await _unitOfWorkRepository.productRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<IEnumerable<ProductDTOs>>> GetAllAsync()
        {
            var response = new ResponseDTOs<IEnumerable<ProductDTOs>>();


            var ProductList = await _unitOfWorkRepository.productRepository.GetAllAsync();
            var result = ProductList.Select(x => new ProductDTOs()
            {
                id = x.Id,
                CategoryId=x.CategoryId,
                Name = x.Name,
            });
            response.Data = result;
            response.Success = true;
            return response;
        }

        public Task<ResponseDTOs<ProductDTOs>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTOs<string>> UpdateAsync(ProductDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
