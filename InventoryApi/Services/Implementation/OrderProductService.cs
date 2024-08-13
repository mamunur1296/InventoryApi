using AutoMapper;
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
        private readonly IMapper _mapper;

        public OrderProductService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(OrderProductDTOs entity)
        {
            var newOrderProduct = new OrderProduct
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = entity.ProductId?.Trim(),

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
            var result = OrderProductList.Select(item => _mapper.Map<OrderProductDTOs>(item));
            return result;
        }

        public async Task<OrderProductDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.orderProductRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Order Product with id = {id} not found");
            }
            var result = _mapper.Map<OrderProductDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, OrderProductDTOs entity)
        {
            var item = await _unitOfWorkRepository.orderProductRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Order Product  with id = {id} not found");
            }
            // Update properties with validation
            item.ProductId=entity.ProductId;

            // Perform update operation
            await _unitOfWorkRepository.orderProductRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }
    }
}
