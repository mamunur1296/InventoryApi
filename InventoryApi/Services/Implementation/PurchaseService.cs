using AutoMapper;
using InventoryApi.DataContext;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services.Implementation
{
    public class PurchaseService : IBaseServices<PurchaseDTOs>,IPurchaseServices
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PurchaseService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper, ApplicationDbContext context)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
            _context = context;
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

        public async Task<bool> PurchaseProduct(PurchaseItemDTOs entitys)
        {
            // Validate the input data
            if (entitys == null || entitys.Products == null || !entitys.Products.Any())
            {
                throw new ValidationException("Invalid purchase item data.");
            }

            // Start a new database transaction
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Create the new purchase record
                var newPurchase = new Purchase
                {
                    Id = Guid.NewGuid().ToString(),
                    CreationDate = DateTime.Now,
                    PurchaseDate = DateTime.Now,
                    SupplierID = entitys.SupplierID,
                };

                // Add the purchase record to the context
                await _context.Purchases.AddAsync(newPurchase);

                // Initialize a list to hold the purchase details
                var purchaseDetails = new List<PurchaseDetail>();

                // Process each product in the purchase
                foreach (var productDto in entitys.Products)
                {
                    var newPurchaseDetail = new PurchaseDetail
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreationDate = DateTime.Now,
                        PurchaseID = newPurchase.Id,
                        ProductID = productDto.ProductID,
                        Quantity = productDto.Quantity,
                        UnitPrice = productDto.Price,
                        Discount = productDto.Discount,
                    };

                    // Add the purchase detail to the list
                    purchaseDetails.Add(newPurchaseDetail);
                }

                // Add all purchase details at once
                await _context.PurchasesDetails.AddRangeAsync(purchaseDetails);

                // Update the stock for each product
                foreach (var productDto in entitys.Products)
                {
                    var product = await _context.Products.FindAsync(productDto.ProductID);

                    if (product != null)
                    {
                        product.UnitsInStock += productDto.Quantity; // Assuming stock should decrease
                        _context.Products.Update(product);
                    }
                }

                // Save all changes and commit the transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Rollback in case of an error
                await transaction.RollbackAsync();
                throw new ValidationException($"An error occurred during the purchase: {ex.Message}");
            }
        }

    }
}
