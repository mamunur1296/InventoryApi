using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;
using System.Xml.Linq;

namespace InventoryApi.Services.Implementation
{
    public class UnitMasterService : IBaseServices<UnitMasterDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public UnitMasterService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(UnitMasterDTOs entity)
        {
            var newUnitMaster = new UnitMaster
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                Name = entity.Name.Trim(),
                UnitMasterDescription = entity.UnitMasterDescription?.Trim(),

            };
            await _unitOfWorkRepository.unitMasterRepository.AddAsync(newUnitMaster);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(string id, UnitMasterDTOs entity)
        {
            var item = await _unitOfWorkRepository.unitMasterRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Unit Master with id = {id} not found");
            }
            // Update properties with validation
            item.Name = entity.Name.Trim();
            item.UnitMasterDescription = entity.UnitMasterDescription?.Trim();

            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.unitMasterRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.unitMasterRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Unit Master with id = {id} not found");
            }
            await _unitOfWorkRepository.unitMasterRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<UnitMasterDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.unitMasterRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<UnitMasterDTOs>(item));
            return result;
        }

        public async Task<UnitMasterDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.unitMasterRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Unit Master with id = {id} not found");
            }
            var result = _mapper.Map<UnitMasterDTOs>(item);
            return result;
        }


    }
}
