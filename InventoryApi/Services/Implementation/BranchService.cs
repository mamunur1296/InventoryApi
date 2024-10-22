using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class BranchService : IBaseServices<BranchDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public BranchService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }
        public async Task<bool> CreateAsync(BranchDTOs entity)
        {
            var newBranch = new Branch
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = _userContextService.UserName,
                CreationDate = DateTime.Now, // Set CreationDate here
                Name = entity.Name?.Trim(),
                FullName = entity.FullName?.Trim(),
                ContactPerson=entity.ContactPerson?.Trim(),
                Address=entity.Address?.Trim(),
                PhoneNo=entity.PhoneNo?.Trim(),
                FaxNo=entity.FaxNo?.Trim(),
                EmailNo= entity.EmailNo?.Trim(),
                IsActive=entity.IsActive,
                CompanyId=entity.CompanyId,
            };
            await _unitOfWorkRepository.branchRepository.AddAsync(newBranch);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(string id, BranchDTOs entity)
        {
            var item = await _unitOfWorkRepository.branchRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($" Branch with id = {id} not found");
            }


            // Update properties with validation
            // item.CartID = string.IsNullOrWhiteSpace(entity.CartID) ? item.CartID : entity.CartID.Trim();

            item.Name = entity.Name?.Trim();
            item.FullName = entity.FullName?.Trim();
            item.ContactPerson = entity.ContactPerson?.Trim();
            item.Address = entity.Address?.Trim();
            item.PhoneNo = entity.PhoneNo?.Trim();
            item.FaxNo = entity.FaxNo?.Trim();
            item.EmailNo = entity.EmailNo?.Trim();
            item.IsActive = entity.IsActive;
            item.CompanyId = entity.CompanyId;
            // Set the UpdateDate to the current date and time
            item.UpdatedBy = _userContextService.UserName;
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.branchRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.branchRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Branch with id = {id} not found");
            }
            await _unitOfWorkRepository.branchRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<BranchDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.branchRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<BranchDTOs>(item));
            return result;
        }

        public async Task<BranchDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.branchRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Branch with id = {id} not found");
            }
            var result = _mapper.Map<BranchDTOs>(item);
            return result;
        }


    }
}
