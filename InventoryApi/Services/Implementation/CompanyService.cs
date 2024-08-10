﻿using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class CompanyService : IBaseServices<CompanyDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public CompanyService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(CompanyDTOs entity)
        {
            var newCompany = new Company
            {
                Id = Guid.NewGuid().ToString(),
                Name = entity.Name?.Trim(),
                FullName = entity.FullName?.Trim(),
                ContactPerson = entity.ContactPerson?.Trim(),
                Address = entity.Address?.Trim(),
                PhoneNo = entity.PhoneNo?.Trim(),
                FaxNo = entity.FaxNo?.Trim(),
                EmailNo = entity.EmailNo?.Trim(),
                IsActive = true // Assuming new companies are always active by default
            };
            await _unitOfWorkRepository.companyRepository.AddAsync(newCompany);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.companyRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            await _unitOfWorkRepository.companyRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<CompanyDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.companyRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<CompanyDTOs>(item));
            return result;
        }

        public async Task<CompanyDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.companyRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            var result = _mapper.Map<CompanyDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, CompanyDTOs entity)
        {
            var item = await _unitOfWorkRepository.companyRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"company with id = {id} not found");
            }
            // Update  properties
            item.Name = string.IsNullOrWhiteSpace(entity.Name) ? item.Name : entity.Name;
            item.FullName = string.IsNullOrWhiteSpace(entity.FullName) ? item.FullName : entity.FullName;
            item.ContactPerson = string.IsNullOrWhiteSpace(entity.ContactPerson) ? item.ContactPerson : entity.ContactPerson;
            item.FaxNo = string.IsNullOrWhiteSpace(entity.FaxNo) ? item.FaxNo : entity.FaxNo;
            item.PhoneNo = string.IsNullOrWhiteSpace(entity.PhoneNo) ? item.PhoneNo : entity.PhoneNo;
            item.EmailNo = string.IsNullOrWhiteSpace(entity.EmailNo) ? item.EmailNo : entity.EmailNo;
            item.IsActive = entity.IsActive; // Assuming IsActive is a non-nullable boolean
            item.Address = string.IsNullOrWhiteSpace(entity.Address) ? item.Address : entity.Address;

            // Perform update operation
            await _unitOfWorkRepository.companyRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }
    }
}
// Helper method for string validation and update
//public static string UpdateProperty(string currentValue, string newValue)
//{
//    return string.IsNullOrWhiteSpace(newValue) ? currentValue : newValue.Trim();
//}

//// Update properties
//company.Name = UpdateProperty(company.Name, entity.Name);
//company.FullName = UpdateProperty(company.FullName, entity.FullName);
//company.ContactPerson = UpdateProperty(company.ContactPerson, entity.ContactPerson);
//company.Address = UpdateProperty(company.Address, entity.Address);
//company.PhoneNo = UpdateProperty(company.PhoneNo, entity.PhoneNo);
//company.FaxNo = UpdateProperty(company.FaxNo, entity.FaxNo);
//company.EmailNo = UpdateProperty(company.EmailNo, entity.EmailNo);
//company.IsActive = entity.IsActive;
