using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class DeliveryAddressService : IBaseServices<DeliveryAddressDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public DeliveryAddressService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }
        public async Task<ResponseDTOs<string>> CreateAsync(DeliveryAddressDTOs entity)
        {
            var response = new ResponseDTOs<string>();
            var newdelivaryAddress = new DeliveryAddress
            {
                Id = Guid.NewGuid().ToString(),
                Address = entity.Address,
                Mobile = entity.Mobile,
                Phone = entity.Phone,
            };
            await _unitOfWorkRepository.deliveryAddressRepository.AddAsync(newdelivaryAddress);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<string>> DeleteAsync(string id)
        {
            var response = new ResponseDTOs<string>();
            var deleteItem = await _unitOfWorkRepository.deliveryAddressRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Delivery Address with id = {id} not found");
            }
            await _unitOfWorkRepository.deliveryAddressRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<IEnumerable<DeliveryAddressDTOs>>> GetAllAsync()
        {
            var response = new ResponseDTOs<IEnumerable<DeliveryAddressDTOs>>();


            var itemList = await _unitOfWorkRepository.deliveryAddressRepository.GetAllAsync();
            var result = itemList.Select(x => new DeliveryAddressDTOs()
            {
                id= x.Id,
                Address = x.Address,
                Mobile = x.Mobile,
                Phone = x.Phone,
            });
            response.Data = result;
            response.Success = true;
            return response;
        }

        public Task<ResponseDTOs<DeliveryAddressDTOs>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTOs<string>> UpdateAsync(DeliveryAddressDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
