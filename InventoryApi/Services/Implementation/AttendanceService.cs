﻿using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class AttendanceService : IBaseServices<AttendanceDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public AttendanceService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(AttendanceDTOs entity)
        {
            var newAttendance = new Attendance
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                
            };
            await _unitOfWorkRepository.attendanceRepository.AddAsync(newAttendance);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(string id, AttendanceDTOs entity)
        {
            var item = await _unitOfWorkRepository.attendanceRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($" Attendance with id = {id} not found");
            }


            // Update properties with validation
            // item.CartID = string.IsNullOrWhiteSpace(entity.CartID) ? item.CartID : entity.CartID.Trim();

            
            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.attendanceRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.attendanceRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Attendance with id = {id} not found");
            }
            await _unitOfWorkRepository.attendanceRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<AttendanceDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.attendanceRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<AttendanceDTOs>(item));
            return result;
        }

        public async Task<AttendanceDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.attendanceRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Attendance with id = {id} not found");
            }
            var result = _mapper.Map<AttendanceDTOs>(item);
            return result;
        }


    }
}
