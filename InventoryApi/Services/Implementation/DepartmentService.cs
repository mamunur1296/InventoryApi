using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class DepartmentService : IBaseServices<DepartmentDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(DepartmentDTOs entity)
        {
            var newDepartment = new Department
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                DepartmentName=entity.DepartmentName,
                Description=entity.Description,
            };
            await _unitOfWorkRepository.departmentRepository.AddAsync(newDepartment);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(string id, DepartmentDTOs entity)
        {
            var item = await _unitOfWorkRepository.departmentRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($" Department with id = {id} not found");
            }


            // Update properties with validation
            // item.CartID = string.IsNullOrWhiteSpace(entity.CartID) ? item.CartID : entity.CartID.Trim();
            item.DepartmentName = entity.DepartmentName;
            item.Description=entity.Description;
            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.departmentRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.departmentRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Department with id = {id} not found");
            }
            await _unitOfWorkRepository.departmentRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<DepartmentDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.departmentRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<DepartmentDTOs>(item));
            return result;
        }

        public async Task<DepartmentDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.departmentRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Department with id = {id} not found");
            }
            var result = _mapper.Map<DepartmentDTOs>(item);
            return result;
        }


    }
}
