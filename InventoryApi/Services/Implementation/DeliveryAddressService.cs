using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;
using Microsoft.AspNetCore.Server.IISIntegration;

namespace InventoryApi.Services.Implementation
{
    public class DeliveryAddressService : IBaseServices<DeliveryAddressDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public DeliveryAddressService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(DeliveryAddressDTOs entity)
        {
            var newdelivaryAddress = new DeliveryAddress
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                Address = entity.Address.Trim(),
                Mobile = entity.Mobile.Trim(),
                Phone = entity.Phone.Trim(),
                UserId = entity.UserId.Trim(),
                DeactivatedDate = entity.DeactivatedDate,
                IsActive = true,
                IsDefault = true,
            };
            await _unitOfWorkRepository.deliveryAddressRepository.AddAsync(newdelivaryAddress);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.deliveryAddressRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Delivery Address with id = {id} not found");
            }
            await _unitOfWorkRepository.deliveryAddressRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<DeliveryAddressDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.deliveryAddressRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<DeliveryAddressDTOs>(item));
            return result;
        }

        public async Task<DeliveryAddressDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.deliveryAddressRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"delivery Address with id = {id} not found");
            }
            var result = _mapper.Map<DeliveryAddressDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, DeliveryAddressDTOs entity)
        {
            var item = await _unitOfWorkRepository.deliveryAddressRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Delivery Address with id = {id} not found");
            }

            // Update properties with validation and trimming
            item.Address = string.IsNullOrWhiteSpace(entity.Address) ? item.Address : entity.Address.Trim();
            item.Mobile = string.IsNullOrWhiteSpace(entity.Mobile) ? item.Mobile : entity.Mobile.Trim();
            item.Phone = string.IsNullOrWhiteSpace(entity.Phone) ? item.Phone : entity.Phone.Trim();
            item.UserId = string.IsNullOrWhiteSpace(entity.UserId) ? item.UserId : entity.UserId.Trim();
            item.IsActive = entity.IsActive;
            item.IsDefault = entity.IsDefault;


            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.deliveryAddressRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
