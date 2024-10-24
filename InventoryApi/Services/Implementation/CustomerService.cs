using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class CustomerService : IBaseServices<CustomerDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<bool> CreateAsync(CustomerDTOs entity)
        {
            var newCustomer = new Customer
            {
               
                CreatedBy = _userContextService.UserName,
                CreationDate = DateTime.Now, // Set CreationDate here
                CustomerName = entity?.CustomerName?.Trim(),
                ContactName = entity?.ContactName?.Trim(),
                ContactTitle = entity?.ContactTitle?.Trim(),
                Address = entity?.Address?.Trim(),
                City = entity?.City?.Trim(),
                Region = entity?.Region?.Trim(),
                PostalCode = entity?.PostalCode?.Trim(),
                Country = entity?.Country?.Trim(),
                Phone = entity?.Phone?.Trim(),
                Fax = entity?.Fax?.Trim(),
                Email = entity?.Email?.Trim(),
                PasswordHash = entity?.PasswordHash?.Trim(),
                DateOfBirth = entity?.DateOfBirth,
                MedicalHistory= entity?.MedicalHistory?.Trim(),
                UserId=entity?.UserId,
                UserName=entity?.UserName,
                Password=entity?.Password,
                
            };
            if (entity.Id != Guid.Empty.ToString() && entity.Id != null)
            {
                newCustomer.Id = entity.Id;
            }
            else
            {
                newCustomer.Id = Guid.NewGuid().ToString();
            }


            await _unitOfWorkRepository.customerRepository.AddAsync(newCustomer);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(string id, CustomerDTOs entity)
        {
            var item = await _unitOfWorkRepository.customerRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Customer with id = {id} not found");
            }

            // Update properties with validation
            item.CustomerName = string.IsNullOrWhiteSpace(entity.CustomerName) ? item.CustomerName : entity.CustomerName.Trim();
            item.ContactName = string.IsNullOrWhiteSpace(entity.ContactName) ? item.ContactName : entity.ContactName.Trim();
            item.ContactTitle = string.IsNullOrWhiteSpace(entity.ContactTitle) ? item.ContactTitle : entity.ContactTitle.Trim();
            item.Address = string.IsNullOrWhiteSpace(entity.Address) ? item.Address : entity.Address.Trim();
            item.City = string.IsNullOrWhiteSpace(entity.City) ? item.City : entity.City.Trim();
            item.Region = string.IsNullOrWhiteSpace(entity.Region) ? item.Region : entity.Region.Trim();
            item.PostalCode = string.IsNullOrWhiteSpace(entity.PostalCode) ? item.PostalCode : entity.PostalCode.Trim();
            item.Country = string.IsNullOrWhiteSpace(entity.Country) ? item.Country : entity.Country.Trim();
            item.Phone = string.IsNullOrWhiteSpace(entity.Phone) ? item.Phone : entity.Phone.Trim();
            item.Fax = string.IsNullOrWhiteSpace(entity.Fax) ? item.Fax : entity.Fax.Trim();
            item.Email = string.IsNullOrWhiteSpace(entity.Email) ? item.Email : entity.Email.Trim();
            item.PasswordHash = string.IsNullOrWhiteSpace(entity.PasswordHash) ? item.PasswordHash : entity.PasswordHash.Trim();
            item.DateOfBirth = entity.DateOfBirth;
            item.MedicalHistory = string.IsNullOrWhiteSpace(entity.MedicalHistory) ? item.MedicalHistory : entity.MedicalHistory.Trim();
            item.UserId= entity.UserId;


            // Set the UpdateDate to the current date and time
            item.UpdatedBy = _userContextService.UserName;
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.customerRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.customerRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Customer with id = {id} not found");
            }
            await _unitOfWorkRepository.customerRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<CustomerDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.customerRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<CustomerDTOs>(item));
            return result;
        }

        public async Task<CustomerDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.customerRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Customer with id = {id} not found");
            }
            var result = _mapper.Map<CustomerDTOs>(item);
            return result;
        }

       
    }
}
