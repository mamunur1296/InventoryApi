using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class PaymentService : IBaseServices<PaymentDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(PaymentDTOs entity)
        {
            var newPayment = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                OrderID = entity.OrderID.Trim(),
                PaymentDate = DateTime.Now,
                PaymentMethod = entity.PaymentMethod.Trim(),
                Amount=entity.Amount,
                PaymentStatus = entity.PaymentStatus.Trim(),
            };
            await _unitOfWorkRepository.paymentRepository.AddAsync(newPayment);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.paymentRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            await _unitOfWorkRepository.paymentRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<PaymentDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.paymentRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<PaymentDTOs>(item));
            return result;
        }

        public async Task<PaymentDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.paymentRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            var result = _mapper.Map<PaymentDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, PaymentDTOs entity)
        {
            var item = await _unitOfWorkRepository.paymentRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Payment with id = {id} not found");
            }

            // Update properties with validation and trimming
            item.OrderID = string.IsNullOrWhiteSpace(entity.OrderID) ? item.OrderID : entity.OrderID.Trim();
            item.PaymentMethod = string.IsNullOrWhiteSpace(entity.PaymentMethod) ? item.PaymentMethod : entity.PaymentMethod.Trim();
            item.Amount = entity.Amount != default ? entity.Amount : item.Amount;
            item.PaymentStatus = string.IsNullOrWhiteSpace(entity.PaymentStatus) ? item.PaymentStatus : entity.PaymentStatus.Trim();

            // Perform update operation
            await _unitOfWorkRepository.paymentRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
