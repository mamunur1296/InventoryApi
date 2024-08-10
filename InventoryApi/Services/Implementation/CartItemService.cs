using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class CartItemService : IBaseServices<CartItemDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public CartItemService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(CartItemDTOs entity)
        {
            var newCartItem = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                CartID = entity.CartID.Trim(),
                Quantity=entity.Quantity,
                ProductID= entity.ProductID.Trim(),
                
            };
            await _unitOfWorkRepository.cartItemRepository.AddAsync(newCartItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(string id, CartItemDTOs entity)
        {
            var item = await _unitOfWorkRepository.cartItemRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Cart item with id = {id} not found");
            }
            // Update properties with validation
            item.CartID = string.IsNullOrWhiteSpace(entity.CartID) ? item.CartID : entity.CartID.Trim();
            item.ProductID = string.IsNullOrWhiteSpace(entity.ProductID) ? item.ProductID : entity.ProductID.Trim();
            item.Quantity = entity.Quantity;

            // Perform update operation
            await _unitOfWorkRepository.cartItemRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.cartItemRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Cart Item with id = {id} not found");
            }
            await _unitOfWorkRepository.cartItemRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<CartItemDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.cartItemRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<CartItemDTOs>(item));
            return result;
        }

        public async Task<CartItemDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.cartItemRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Cart item with id = {id} not found");
            }
            var result = _mapper.Map<CartItemDTOs>(item);
            return result;
        }

        
    }
}
