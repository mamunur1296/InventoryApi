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

        public CompanyService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<bool> CreateAsync(CompanyDTOs entity)
        {
            var newCompany = new Company
            {
                Id = Guid.NewGuid().ToString(),
                Name = entity.Name,
                FullName=entity.FullName,
                ContactPerson=entity.ContactPerson,
                Address=entity.Address,
                PhoneNo=entity.PhoneNo,
                FaxNo=entity.FaxNo,
                EmailNo=entity.EmailNo,
                IsActive=true
            };
            await _unitOfWorkRepository.companyRepository.AddAsync(newCompany);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteCompany = await _unitOfWorkRepository.companyRepository.GetByIdAsync(id);

            if (deleteCompany == null)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            await _unitOfWorkRepository.companyRepository.DeleteAsync(deleteCompany);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<CompanyDTOs>> GetAllAsync()
        {
            var companyList = await _unitOfWorkRepository.companyRepository.GetAllAsync();
            var result = companyList.Select(x => new CompanyDTOs()
            {
                Id=x.Id,
                Name=x.Name,
                IsActive = x.IsActive,
                EmailNo=x.EmailNo,
                FaxNo=x.FaxNo,
                PhoneNo=x.PhoneNo,
                Address=x.Address,
                ContactPerson=x.ContactPerson,
                FullName = x.FullName   
            });
            return result;
        }

        public async Task<CompanyDTOs> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(CompanyDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
