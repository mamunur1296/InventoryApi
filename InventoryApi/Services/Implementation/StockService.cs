using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class StockService : IBaseServices<StockDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public StockService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(StockDTOs entity)
        {
            var newStock = new Stock
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                ProductID = entity.ProductID.Trim(),
                WarehouseID= entity.WarehouseID.Trim(),
                Quantity=entity.Quantity
            };
            await _unitOfWorkRepository.stockRepository.AddAsync(newStock);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.stockRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            await _unitOfWorkRepository.stockRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<StockDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.stockRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<StockDTOs>(item));
            return result;
        }

        public async Task<StockDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.stockRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            var result = _mapper.Map<StockDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, StockDTOs entity)
        {
            var item = await _unitOfWorkRepository.stockRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Stock with id = {id} not found");
            }

            // Update properties with validation
            item.ProductID = string.IsNullOrWhiteSpace(entity.ProductID) ? item.ProductID : entity.ProductID.Trim();
            item.WarehouseID = string.IsNullOrWhiteSpace(entity.WarehouseID) ? item.WarehouseID : entity.WarehouseID.Trim();
            item.Quantity = entity.Quantity;



            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.stockRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
