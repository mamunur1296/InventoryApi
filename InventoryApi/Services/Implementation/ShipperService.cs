using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class ShipperService : IBaseServices<ShipperDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public ShipperService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(ShipperDTOs entity)
        {
            var newShipper = new Shipper
            {
                Id = Guid.NewGuid().ToString(),
                ShipperName = entity.ShipperName.Trim(),
                Phone = entity.Phone.Trim(),
            };
            await _unitOfWorkRepository.shipperRepository.AddAsync(newShipper);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.shipperRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            await _unitOfWorkRepository.shipperRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<ShipperDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.shipperRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<ShipperDTOs>(item));
            return result;
        }

        public async Task<ShipperDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.shipperRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            var result = _mapper.Map<ShipperDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, ShipperDTOs entity)
        {
            var item = await _unitOfWorkRepository.shipperRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Shipper with id = {id} not found");
            }

            // Update properties with validation and trimming
            item.ShipperName = string.IsNullOrWhiteSpace(entity.ShipperName) ? item.ShipperName : entity.ShipperName.Trim();
            item.Phone = string.IsNullOrWhiteSpace(entity.Phone) ? item.Phone : entity.Phone.Trim();

            // Perform update operation
            await _unitOfWorkRepository.shipperRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
