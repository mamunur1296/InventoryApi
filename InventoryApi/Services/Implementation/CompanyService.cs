using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;
using System.Net;

namespace InventoryApi.Services.Implementation
{
    public class CompanyService : IBaseServices<CompanyDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public CompanyService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<ResponseDTOs<string>> CreateAsync(CompanyDTOs entity)
        {
            var response = new ResponseDTOs<string>();
            var newCompany = new Company
            {
                Id = Guid.NewGuid().ToString(),
                Name = entity.Name,
            };
            await _unitOfWorkRepository.companyRepository.AddAsync(newCompany);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<string>> DeleteAsync(string id)
        {

            var response = new ResponseDTOs<string>();
            var deleteCompany = await _unitOfWorkRepository.companyRepository.GetByIdAsync(id);

            if (deleteCompany == null)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            await _unitOfWorkRepository.companyRepository.DeleteAsync(deleteCompany);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<IEnumerable<CompanyDTOs>>> GetAllAsync()
        {
            var response = new ResponseDTOs<IEnumerable<CompanyDTOs>>();


            var companyList = await _unitOfWorkRepository.companyRepository.GetAllAsync();
            var result = companyList.Select(x => new CompanyDTOs()
            {
                Id=x.Id,
                Name=x.Name,
            });
            response.Data = result;
            response.Success=true;
            return response;
        }

        public Task<ResponseDTOs<CompanyDTOs>> GetByIdAsync(string id)
        {
           
            throw new NotImplementedException();
        }

        public Task<ResponseDTOs<string>> UpdateAsync(CompanyDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
