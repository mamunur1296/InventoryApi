using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class ShoppingCartService : IBaseServices<ShoppingCartDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public ShoppingCartService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(ShoppingCartDTOs entity)
        {
            var newShoppingCart = new ShoppingCart
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                CustomerID = entity.CustomerID.Trim(),
                CreatedDate = DateTime.Now,
            };
            await _unitOfWorkRepository.shoppingCartRepository.AddAsync(newShoppingCart);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.shoppingCartRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            await _unitOfWorkRepository.shoppingCartRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<ShoppingCartDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.shoppingCartRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<ShoppingCartDTOs>(item));
            return result;
        }

        public async Task<ShoppingCartDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.shoppingCartRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            var result = _mapper.Map<ShoppingCartDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, ShoppingCartDTOs entity)
        {
            var item = await _unitOfWorkRepository.shoppingCartRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Shopping cart with id = {id} not found");
            }

            // Update properties with validation
            item.CustomerID = string.IsNullOrWhiteSpace(entity.CustomerID) ? item.CustomerID : entity.CustomerID;
            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);

            // Perform update operation
            await _unitOfWorkRepository.shoppingCartRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
