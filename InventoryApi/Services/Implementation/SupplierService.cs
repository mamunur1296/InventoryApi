using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class SupplierService : IBaseServices<SupplierDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public SupplierService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(SupplierDTOs entity)
        {
            var newSupplier = new Supplier
            {
                Id = Guid.NewGuid().ToString(),
                SupplierName = entity.SupplierName.Trim(),
                ContactName = entity.ContactName.Trim(),
                ContactTitle = entity.ContactTitle.Trim(),
                Address = entity.Address.Trim(),
                City= entity.City.Trim(),
                Region= entity.Region.Trim(),
                PostalCode = entity.PostalCode.Trim(),
                Country= entity.Country.Trim(),
                Phone= entity.Phone.Trim(),
                Fax= entity.Fax.Trim(),
                HomePage= entity.HomePage.Trim(),
            };
            await _unitOfWorkRepository.supplierRepository.AddAsync(newSupplier);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.supplierRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            await _unitOfWorkRepository.supplierRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<SupplierDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.supplierRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<SupplierDTOs>(item));
            return result;
        }

        public async Task<SupplierDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.supplierRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            var result = _mapper.Map<SupplierDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, SupplierDTOs entity)
        {
            var item = await _unitOfWorkRepository.supplierRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Supplier with id = {id} not found");
            }

            // Update properties with validation
            item.SupplierName = string.IsNullOrWhiteSpace(entity.SupplierName) ? item.SupplierName : entity.SupplierName.Trim();
            item.ContactName = string.IsNullOrWhiteSpace(entity.ContactName) ? item.ContactName : entity.ContactName.Trim();
            item.ContactTitle = string.IsNullOrWhiteSpace(entity.ContactTitle) ? item.ContactTitle : entity.ContactTitle.Trim();
            item.Address = string.IsNullOrWhiteSpace(entity.Address) ? item.Address : entity.Address.Trim();
            item.City = string.IsNullOrWhiteSpace(entity.City) ? item.City : entity.City.Trim();
            item.Region = string.IsNullOrWhiteSpace(entity.Region) ? item.Region : entity.Region.Trim();
            item.PostalCode = string.IsNullOrWhiteSpace(entity.PostalCode) ? item.PostalCode : entity.PostalCode.Trim();
            item.Country = string.IsNullOrWhiteSpace(entity.Country) ? item.Country : entity.Country.Trim();
            item.Phone = string.IsNullOrWhiteSpace(entity.Phone) ? item.Phone : entity.Phone.Trim();
            item.Fax = string.IsNullOrWhiteSpace(entity.Fax) ? item.Fax : entity.Fax.Trim();
            item.HomePage = string.IsNullOrWhiteSpace(entity.HomePage) ? item.HomePage : entity.HomePage.Trim();

            // Perform update operation
            await _unitOfWorkRepository.supplierRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
