using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class PurchaseDetailService : IBaseServices<PurchaseDetailDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public PurchaseDetailService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(PurchaseDetailDTOs entity)
        {
            var newBranch = new PurchaseDetail
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                PurchaseID= entity.PurchaseID,
                ProductID= entity.ProductID,
                Quantity = entity.Quantity,
                UnitPrice = entity.UnitPrice,
                Discount = entity.Discount,
            };
            await _unitOfWorkRepository.purchaseDetailRepository.AddAsync(newBranch);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(string id, PurchaseDetailDTOs entity)
        {
            var item = await _unitOfWorkRepository.purchaseDetailRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($" Purchase Detail with id = {id} not found");
            }


            // Update properties with validation
            // item.CartID = string.IsNullOrWhiteSpace(entity.CartID) ? item.CartID : entity.CartID.Trim();
            item.PurchaseID= entity.PurchaseID;
            item.ProductID= entity.ProductID;
            item.Quantity = entity.Quantity;
            item.UnitPrice = entity.UnitPrice;
            item.Discount = entity.Discount;
            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.purchaseDetailRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.purchaseDetailRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Purchase Detail with id = {id} not found");
            }
            await _unitOfWorkRepository.purchaseDetailRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<PurchaseDetailDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.purchaseDetailRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<PurchaseDetailDTOs>(item));
            return result;
        }

        public async Task<PurchaseDetailDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.purchaseDetailRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Purchase Detail with id = {id} not found");
            }
            var result = _mapper.Map<PurchaseDetailDTOs>(item);
            return result;
        }


    }
}
