using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class EmployeeService : IBaseServices<EmployeeDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(EmployeeDTOs entity)
        {
            var newEmployee = new Employee
            {
                Id = entity.Id,
                CreatedBy = entity?.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                FirstName = entity.FirstName.Trim(),
                LastName = entity.LastName.Trim(),
                Title = string.IsNullOrWhiteSpace(entity.Title) ? null : entity.Title.Trim(),
                TitleOfCourtesy = string.IsNullOrWhiteSpace(entity.TitleOfCourtesy) ? null : entity.TitleOfCourtesy.Trim(),
                BirthDate = entity?.BirthDate,
                HireDate = DateTime.Now,
                Address = string.IsNullOrWhiteSpace(entity?.Address) ? null : entity?.Address.Trim(),
                City = string.IsNullOrWhiteSpace(entity?.City) ? null : entity.City.Trim(),
                Region = string.IsNullOrWhiteSpace(entity?.Region) ? null : entity.Region.Trim(),
                PostalCode = string.IsNullOrWhiteSpace(entity?.PostalCode) ? null : entity.PostalCode.Trim(),
                Country = string.IsNullOrWhiteSpace(entity?.Country) ? null : entity.Country.Trim(),
                HomePhone = string.IsNullOrWhiteSpace(entity.HomePhone) ? null : entity.HomePhone.Trim(),
                Extension = string.IsNullOrWhiteSpace(entity.Extension) ? null : entity.Extension.Trim(),
                Photo = entity?.Photo, // Assuming Photo is nullable
                Notes = string.IsNullOrWhiteSpace(entity?.Notes) ? null : entity?.Notes?.Trim(),
                ReportsTo = entity?.ReportsTo,
                PhotoPath = string.IsNullOrWhiteSpace(entity?.PhotoPath) ? null : entity?.PhotoPath.Trim(),
                ManagerId=entity?.ManagerId?.Trim(),
                UserId=entity?.UserId,
                BranchId=entity?.BranchId,
                CompanyId=entity?.CompanyId,
            };
            
            await _unitOfWorkRepository.employeeRepository.AddAsync(newEmployee);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }


        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.employeeRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Employee with id = {id} not found");
            }
            await _unitOfWorkRepository.employeeRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<EmployeeDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.employeeRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<EmployeeDTOs>(item));
            return result;
        }

        public async Task<EmployeeDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.employeeRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Employee with id = {id} not found");
            }
            var result = _mapper.Map<EmployeeDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, EmployeeDTOs entity)
        {
            var item = await _unitOfWorkRepository.employeeRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Employee with id = {id} not found");
            }

            // Update properties with validation and trimming
            item.FirstName =  entity.FirstName.Trim();
            item.LastName = entity.LastName.Trim();
            item.Title =  entity?.Title?.Trim();
            item.TitleOfCourtesy =  entity?.TitleOfCourtesy?.Trim();
            item.BirthDate =    item.BirthDate;
            item.HireDate = item.HireDate;
            item.Address =  entity?.Address?.Trim();
            item.City =  entity?.City?.Trim();
            item.Region = entity?.Region?.Trim();
            item.PostalCode =  entity?.PostalCode?.Trim();
            item.Country = entity?.Country?.Trim();
            item.HomePhone =  entity?.HomePhone?.Trim();
            item.Extension =  entity?.Extension?.Trim();
            item.Photo = entity?.Photo ;
            item.Notes =  entity?.Notes?.Trim();
            item.ReportsTo = entity?.ReportsTo ?? item.ReportsTo;
            item.PhotoPath = entity?.PhotoPath?.Trim();
            item.ManagerId = entity?.ManagerId?.Trim();
            item.UserId = entity?.UserId;
            item.CompanyId = entity?.CompanyId?.Trim();
            item.BranchId = entity?.BranchId?.Trim();   


            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity?.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.employeeRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
