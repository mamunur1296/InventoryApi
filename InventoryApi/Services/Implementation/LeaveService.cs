using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class LeaveService : IBaseServices<LeaveDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public LeaveService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(LeaveDTOs entity)
        {
            var newLeave = new Leave
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
               
            };
            await _unitOfWorkRepository.leaveRepository.AddAsync(newLeave);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(string id, LeaveDTOs entity)
        {
            var item = await _unitOfWorkRepository.leaveRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($" Leave with id = {id} not found");
            }


            // Update properties with validation
            // item.CartID = string.IsNullOrWhiteSpace(entity.CartID) ? item.CartID : entity.CartID.Trim();

            
            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.leaveRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.leaveRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Leave with id = {id} not found");
            }
            await _unitOfWorkRepository.leaveRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<LeaveDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.leaveRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<LeaveDTOs>(item));
            return result;
        }

        public async Task<LeaveDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.leaveRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Leave with id = {id} not found");
            }
            var result = _mapper.Map<LeaveDTOs>(item);
            return result;
        }


    }
}
