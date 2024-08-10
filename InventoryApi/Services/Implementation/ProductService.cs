using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class ProductService : IBaseServices<ProductDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(ProductDTOs entity)
        {
            var newProduct = new Product
            {
                Id = Guid.NewGuid().ToString(),
                ProductName = entity.ProductName.Trim(),
                Description = entity.Description.Trim(),
                CategoryID = entity.CategoryID.ToString(),
                SupplierID = entity.SupplierID.ToString(),
                QuantityPerUnit = entity.QuantityPerUnit.ToString(),
                UnitPrice=entity.UnitPrice,
                UnitsInStock=entity.UnitsInStock,
                ReorderLevel=entity.ReorderLevel,
                Discontinued=entity.Discontinued,
                BatchNumber=entity.BatchNumber,
                ExpirationDate=entity.ExpirationDate,
                ImageURL=entity.ImageURL.Trim(),
                Weight=entity.Weight,
                Dimensions=entity.Dimensions,
            };
            await _unitOfWorkRepository.productRepository.AddAsync(newProduct);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.productRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($" Product   with id = {id} not found");
            }
            await _unitOfWorkRepository.productRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<ProductDTOs>> GetAllAsync()
        {
            var items = await _unitOfWorkRepository.productRepository.GetAllAsync();
            var result = items.Select(item => _mapper.Map<ProductDTOs>(item));
            return result;
        }

        public async Task<ProductDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.productRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            var result = _mapper.Map<ProductDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, ProductDTOs entity)
        {
            var item = await _unitOfWorkRepository.productRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Product with id = {id} not found");
            }

            // Update properties with validation and trimming
            item.ProductName = string.IsNullOrWhiteSpace(entity.ProductName) ? item.ProductName : entity.ProductName.Trim();
            item.Description = string.IsNullOrWhiteSpace(entity.Description) ? item.Description : entity.Description.Trim();
            item.CategoryID = entity.CategoryID != null ? entity.CategoryID.ToString() : item.CategoryID;
            item.SupplierID = entity.SupplierID != null ? entity.SupplierID.ToString() : item.SupplierID;
            item.QuantityPerUnit = entity.QuantityPerUnit != null ? entity.QuantityPerUnit.ToString() : item.QuantityPerUnit;
            item.UnitPrice = entity?.UnitPrice != null ? entity.UnitPrice : item.UnitPrice;
            item.UnitsInStock = entity?.UnitsInStock != null ? entity.UnitsInStock : item.UnitsInStock;
            item.ReorderLevel = entity?.ReorderLevel != null ? entity.ReorderLevel : item.ReorderLevel;
            item.Discontinued = entity?.Discontinued != null ? entity.Discontinued : item.Discontinued;
            item.BatchNumber = entity?.BatchNumber != null ? entity.BatchNumber : item.BatchNumber;
            item.ExpirationDate = entity?.ExpirationDate != null ? entity.ExpirationDate : item.ExpirationDate;
            item.ImageURL = string.IsNullOrWhiteSpace(entity?.ImageURL) ? item.ImageURL : entity.ImageURL.Trim();
            item.Weight = entity?.Weight != null ? entity.Weight : item.Weight;
            item.Dimensions = entity?.Dimensions != null ? entity.Dimensions : item.Dimensions;

            // Perform update operation
            await _unitOfWorkRepository.productRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
