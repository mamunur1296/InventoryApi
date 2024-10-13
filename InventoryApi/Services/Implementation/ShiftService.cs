using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class ShiftService : IBaseServices<ShiftDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public ShiftService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(ShiftDTOs entity)
        {
            var newShift = new Shift
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                ShiftName = entity.ShiftName,
                StartTime=entity.StartTime,
                EndTime=entity.EndTime,

               
            };
            await _unitOfWorkRepository.shiftRepository.AddAsync(newShift);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(string id, ShiftDTOs entity)
        {
            var item = await _unitOfWorkRepository.shiftRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($" Shift with id = {id} not found");
            }
            // Update properties with validation
            // item.CartID = string.IsNullOrWhiteSpace(entity.CartID) ? item.CartID : entity.CartID.Trim();
            item.ShiftName=entity.ShiftName; 
            item.StartTime=entity.StartTime; 
            item.EndTime=entity.EndTime;
            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.shiftRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.shiftRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Shift with id = {id} not found");
            }
            await _unitOfWorkRepository.shiftRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<IEnumerable<ShiftDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.shiftRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<ShiftDTOs>(item));
            return result;
        }
        public async Task<ShiftDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.shiftRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Shift with id = {id} not found");
            }
            var result = _mapper.Map<ShiftDTOs>(item);
            return result;
        }
    }
}
