using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class OrderService : IBaseServices<OrderDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(OrderDTOs entity)
        {
            var newOrder = new Order
            {
                Id = Guid.NewGuid().ToString(),
                CustomerID = entity.CustomerID.Trim(),
                EmployeeID = entity.EmployeeID?.Trim(),
                OrderDate = DateTime.Now,
                RequiredDate = entity.RequiredDate,
                ShippedDate = entity.ShippedDate,
                ShipVia = entity.ShipVia,
                Freight= entity.Freight,
                ShipName = entity.ShipName.Trim(),
                ShipAddress = entity.ShipAddress.Trim(),
                ShipCity = entity.ShipCity.Trim(),
                ShipRegion = entity.ShipRegion.Trim(),
                ShipPostalCode = entity.ShipPostalCode.Trim(),
                ShipCountry = entity.ShipCountry.Trim(),
                PrescriptionID=entity.PrescriptionID?.Trim(),
                PaymentStatus=entity.PaymentStatus.Trim(),
                OrderStatus=entity.OrderStatus.Trim(),
            };
            await _unitOfWorkRepository.orderRepository.AddAsync(newOrder);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.orderRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Order  with id = {id} not found");
            }
            await _unitOfWorkRepository.orderRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<OrderDTOs>> GetAllAsync()
        {
            var OrderList = await _unitOfWorkRepository.orderRepository.GetAllAsync();
            var result = OrderList.Select(x => _mapper.Map<OrderDTOs>(x));
            return result;
        }

        public async Task<OrderDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.orderRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"order with id = {id} not found");
            }
            var result = _mapper.Map<OrderDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, OrderDTOs entity)
        {
            var item = await _unitOfWorkRepository.orderRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Order with id = {id} not found");
            }

            // Update properties with validation and trimming
            item.CustomerID = string.IsNullOrWhiteSpace(entity.CustomerID) ? item.CustomerID : entity.CustomerID.Trim();
            item.EmployeeID = string.IsNullOrWhiteSpace(entity.EmployeeID) ? item.EmployeeID : entity.EmployeeID.Trim();
            item.RequiredDate = entity.RequiredDate != default ? entity.RequiredDate : item.RequiredDate;
            item.ShippedDate = entity.ShippedDate ?? item.ShippedDate;
            item.ShipVia = entity.ShipVia != default ? entity.ShipVia : item.ShipVia;
            item.Freight = entity.Freight != default ? entity.Freight : item.Freight;
            item.ShipName = string.IsNullOrWhiteSpace(entity.ShipName) ? item.ShipName : entity.ShipName.Trim();
            item.ShipAddress = string.IsNullOrWhiteSpace(entity.ShipAddress) ? item.ShipAddress : entity.ShipAddress.Trim();
            item.ShipCity = string.IsNullOrWhiteSpace(entity.ShipCity) ? item.ShipCity : entity.ShipCity.Trim();
            item.ShipRegion = string.IsNullOrWhiteSpace(entity.ShipRegion) ? item.ShipRegion : entity.ShipRegion.Trim();
            item.ShipPostalCode = string.IsNullOrWhiteSpace(entity.ShipPostalCode) ? item.ShipPostalCode : entity.ShipPostalCode.Trim();
            item.ShipCountry = string.IsNullOrWhiteSpace(entity.ShipCountry) ? item.ShipCountry : entity.ShipCountry.Trim();
            item.PrescriptionID = string.IsNullOrWhiteSpace(entity.PrescriptionID) ? item.PrescriptionID : entity.PrescriptionID.Trim();
            item.PaymentStatus = string.IsNullOrWhiteSpace(entity.PaymentStatus) ? item.PaymentStatus : entity.PaymentStatus.Trim();
            item.OrderStatus = string.IsNullOrWhiteSpace(entity.OrderStatus) ? item.OrderStatus : entity.OrderStatus.Trim();

            // Perform update operation
            await _unitOfWorkRepository.orderRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
