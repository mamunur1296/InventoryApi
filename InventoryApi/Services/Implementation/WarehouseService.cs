using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class WarehouseService : IBaseServices<WarehouseDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public WarehouseService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(WarehouseDTOs entity)
        {
            var newWarehouse = new Warehouse
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                WarehouseName =entity.WarehouseName.Trim(),
                Location = entity.Location.Trim(),
                CompanyId=entity?.CompanyId?.Trim(),
            };
            await _unitOfWorkRepository.warehouseRepository.AddAsync(newWarehouse);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.warehouseRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($" Warehouse with id = {id} not found");
            }
            await _unitOfWorkRepository.warehouseRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<WarehouseDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.warehouseRepository.GetAllAsync();
            var result = itemList.Select(item =>_mapper.Map<WarehouseDTOs>(item));
            return result;
        }

        public async Task<WarehouseDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.warehouseRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            var result = _mapper.Map<WarehouseDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, WarehouseDTOs entity)
        {
            var item = await _unitOfWorkRepository.warehouseRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Warehouse with id = {id} not found");
            }

            // Update properties with validation
            item.WarehouseName = string.IsNullOrWhiteSpace(entity.WarehouseName) ? item.WarehouseName : entity.WarehouseName.Trim();
            item.Location = string.IsNullOrWhiteSpace(entity.Location) ? item.Location : entity.Location.Trim();
            item.CompanyId = string.IsNullOrWhiteSpace(entity.CompanyId) ? item.CompanyId : entity.CompanyId.Trim(); 

            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.warehouseRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
