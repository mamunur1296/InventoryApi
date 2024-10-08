using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class HolidayService : IBaseServices<HolidayDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public HolidayService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(HolidayDTOs entity)
        {
            var newHoliday = new Holiday
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                
            };
            await _unitOfWorkRepository.holidayRepository.AddAsync(newHoliday);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(string id, HolidayDTOs entity)
        {
            var item = await _unitOfWorkRepository.holidayRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($" Holiday with id = {id} not found");
            }


            // Update properties with validation
            // item.CartID = string.IsNullOrWhiteSpace(entity.CartID) ? item.CartID : entity.CartID.Trim();

          
            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.holidayRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.holidayRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Holiday with id = {id} not found");
            }
            await _unitOfWorkRepository.holidayRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<HolidayDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.holidayRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<HolidayDTOs>(item));
            return result;
        }

        public async Task<HolidayDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.holidayRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Holiday with id = {id} not found");
            }
            var result = _mapper.Map<HolidayDTOs>(item);
            return result;
        }


    }
}
