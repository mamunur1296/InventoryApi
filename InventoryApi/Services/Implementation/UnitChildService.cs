using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;
using System.Xml.Linq;

namespace InventoryApi.Services.Implementation
{
    public class UnitChildService : IBaseServices<UnitChildhDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public UnitChildService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }
        public async Task<bool> CreateAsync(UnitChildhDTOs entity)
        {
            var newUnitChild = new UnitChild
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = _userContextService.UserName,
                CreationDate = DateTime.Now, // Set CreationDate here
                Name= entity.Name.Trim(),
                UnitMasterId= entity.UnitMasterId.Trim(),
                DisplayName= entity.DisplayName?.Trim(),
                UnitDescription= entity.UnitDescription?.Trim(),
                UnitShortCode=entity.UnitShortCode.Trim(),

            };
            await _unitOfWorkRepository.unitChildRepository.AddAsync(newUnitChild);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(string id, UnitChildhDTOs entity)
        {
            var item = await _unitOfWorkRepository.unitChildRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Unit Child with id = {id} not found");
            }
            // Update properties with validation

            item.Name = entity.Name.Trim();
            item.UnitMasterId = entity.UnitMasterId.Trim();
            item.DisplayName = entity.DisplayName?.Trim();
            item.UnitDescription = entity.UnitDescription?.Trim();
            // Set the UpdateDate to the current date and time
            item.UpdatedBy = _userContextService.UserName;
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.unitChildRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.unitChildRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Unit Child with id = {id} not found");
            }
            await _unitOfWorkRepository.unitChildRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<UnitChildhDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.unitChildRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<UnitChildhDTOs>(item));
            return result;
        }

        public async Task<UnitChildhDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.unitChildRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Unit Child with id = {id} not found");
            }
            var result = _mapper.Map<UnitChildhDTOs>(item);
            return result;
        }
    }
}
