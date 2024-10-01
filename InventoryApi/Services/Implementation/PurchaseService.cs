using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class PurchaseService : IBaseServices<PurchaseDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public PurchaseService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(PurchaseDTOs entity)
        {
            var newBranch = new Purchase
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                PurchaseDate = DateTime.Now,
                SupplierID = entity.SupplierID,
                TotalAmount = entity.TotalAmount,
            };
            await _unitOfWorkRepository.purchaseRepository.AddAsync(newBranch);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(string id, PurchaseDTOs entity)
        {
            var item = await _unitOfWorkRepository.purchaseRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($" Purchase  with id = {id} not found");
            }


            // Update properties with validation
            // item.CartID = string.IsNullOrWhiteSpace(entity.CartID) ? item.CartID : entity.CartID.Trim();


            item.SupplierID = entity.SupplierID;
            item.TotalAmount = entity.TotalAmount;


            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.purchaseRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.purchaseRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Purchase  with id = {id} not found");
            }
            await _unitOfWorkRepository.purchaseRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<PurchaseDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.purchaseRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<PurchaseDTOs>(item));
            return result;
        }

        public async Task<PurchaseDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.purchaseRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Purchase with id = {id} not found");
            }
            var result = _mapper.Map<PurchaseDTOs>(item);
            return result;
        }


    }
}
