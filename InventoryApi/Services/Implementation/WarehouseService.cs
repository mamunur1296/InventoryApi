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

        public WarehouseService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<bool> CreateAsync(WarehouseDTOs entity)
        {
            var newWarehouse = new Warehouse
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = entity.CompanyId,
                Name=entity.Name,
                Address= entity.Address,
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
            var companyList = await _unitOfWorkRepository.warehouseRepository.GetAllAsync();
            var result = companyList.Select(x => new WarehouseDTOs()
            {
                id = x.Id,
                CompanyId = x.CompanyId, 
                Address = x.Address,
                Name=x.Name,
            });
            return result;
        }

        public Task<WarehouseDTOs> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(WarehouseDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
