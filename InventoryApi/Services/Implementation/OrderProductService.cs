using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class OrderProductService : IBaseServices<OrderProductDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public OrderProductService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<bool> CreateAsync(OrderProductDTOs entity)
        {
            var newOrderProduct = new OrderProduct
            {
                Id = Guid.NewGuid().ToString(),
                //Order=entity.Order,
               // Product=entity.Product,

            };
            await _unitOfWorkRepository.orderProductRepository.AddAsync(newOrderProduct);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.orderProductRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Order Product   with id = {id} not found");
            }
            await _unitOfWorkRepository.orderProductRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<OrderProductDTOs>> GetAllAsync()
        {
            var OrderProductList = await _unitOfWorkRepository.orderProductRepository.GetAllAsync();
            var result = OrderProductList.Select(x => new OrderProductDTOs()
            {
                id = x.Id,
               //Order=x.Order,
              // Product=x.Product,
            });
            return result;
        }

        public Task<OrderProductDTOs> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(OrderProductDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
