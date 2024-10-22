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
        private readonly IUserContextService _userContextService;
        public ProductService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<bool> CreateAsync(ProductDTOs entity)
        {
            var newProduct = new Product
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = _userContextService.UserName,
                CreationDate = DateTime.Now, // Set CreationDate here
                ProductName = entity?.ProductName?.Trim(),
                Description = entity?.Description?.Trim(),
                CategoryID = entity?.CategoryID?.ToString(),
                SupplierID = entity?.SupplierID?.ToString(),
                QuantityPerUnit = entity?.QuantityPerUnit?.ToString(),
                UnitPrice=entity.UnitPrice,
                UnitsInStock=entity?.UnitsInStock,
                ReorderLevel=entity?.ReorderLevel,
                Discontinued=entity.Discontinued,
                BatchNumber=entity?.BatchNumber,
                ExpirationDate=entity?.ExpirationDate,
                ImageURL=entity?.ImageURL?.Trim(),
                Weight=entity?.Weight,
                Dimensions=entity?.Dimensions,
                UnitChildId=entity?.UnitChildId,
                UnitMasterId=entity?.UnitMasterId,
                Discount=entity?.Discount,
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
            item.ProductName = entity.ProductName.Trim();
            item.Description =  entity?.Description?.Trim();
            item.CategoryID = entity.CategoryID ;
            item.SupplierID = entity.SupplierID ;
            item.QuantityPerUnit = entity.QuantityPerUnit ;
            item.UnitPrice = entity.UnitPrice ;
            item.UnitsInStock = entity?.UnitsInStock ;
            item.ReorderLevel = entity?.ReorderLevel ;
            item.Discontinued = entity.Discontinued ;
            item.BatchNumber = entity?.BatchNumber ;
            item.ExpirationDate = entity?.ExpirationDate ;
            item.ImageURL = entity?.ImageURL?.Trim();
            item.Weight = entity?.Weight ;
            item.Dimensions = entity?.Dimensions ;
            item.UnitMasterId = entity?.UnitMasterId;
            item.UnitChildId=entity?.UnitChildId;
            item.Discount=entity?.Discount ;


            // Set the UpdateDate to the current date and time
            item.UpdatedBy = _userContextService.UserName;
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.productRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
