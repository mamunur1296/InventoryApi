using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class OrderService : IBaseServices<OrderDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public OrderService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<bool> CreateAsync(OrderDTOs entity)
        {
            var newOrder = new Order
            {
                Id = Guid.NewGuid().ToString(),
                OrderDate = DateTime.Now,
               // OrderProducts = entity.OrderProducts,

            };
            await _unitOfWorkRepository.orderRepository.AddAsync(newOrder);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.orderRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Order  with id = {id} not found");
            }
            await _unitOfWorkRepository.orderRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<OrderDTOs>> GetAllAsync()
        {
            var OrderList = await _unitOfWorkRepository.orderRepository.GetAllAsync();
            var result = OrderList.Select(x => new OrderDTOs()
            {
                Id=x.Id,
               // OrderProducts=x.OrderProducts,
                OrderDate=x.OrderDate,  
            });
            return result;
        }

        public Task<OrderDTOs> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(OrderDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
