using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class OrderDetailService : IBaseServices<OrderDetailDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public OrderDetailService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(OrderDetailDTOs entity)
        {
            var newOrderDetail = new OrderDetail
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                OrderID =entity.OrderID.Trim(),
                ProductID=entity.ProductID.Trim(),
                UnitPrice=entity.UnitPrice,
                Quantity=entity.Quantity,
                Discount=entity.Discount,
               
            };
            await _unitOfWorkRepository.orderDetailRepository.AddAsync(newOrderDetail);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.orderDetailRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Order Detail with id = {id} not found");
            }
            await _unitOfWorkRepository.orderDetailRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<OrderDetailDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.orderDetailRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<OrderDetailDTOs>(item));
            return result;
        }

        public async Task<OrderDetailDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.orderDetailRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Order Detail with id = {id} not found");
            }
            var result = _mapper.Map<OrderDetailDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, OrderDetailDTOs entity)
        {
            var item = await _unitOfWorkRepository.orderDetailRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Order Detail with id = {id} not found");
            }

            // Update properties with validation and trimming
            item.OrderID = string.IsNullOrWhiteSpace(entity.OrderID) ? item.OrderID : entity.OrderID.Trim();
            item.ProductID = string.IsNullOrWhiteSpace(entity.ProductID) ? item.ProductID : entity.ProductID.Trim();
            item.UnitPrice = entity.UnitPrice != default ? entity.UnitPrice : item.UnitPrice;
            item.Quantity = entity.Quantity != default ? entity.Quantity : item.Quantity;
            item.Discount = entity.Discount != default ? entity.Discount : item.Discount;


            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.orderDetailRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
