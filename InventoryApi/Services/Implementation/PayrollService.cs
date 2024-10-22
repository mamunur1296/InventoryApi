using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class PayrollService : IBaseServices<PayrollDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public PayrollService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }
        public async Task<bool> CreateAsync(PayrollDTOs entity)
        {
            var newPayroll = new Payroll
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = _userContextService.UserName,
                CreationDate = DateTime.Now, // Set CreationDate here
                EmployeeId = entity.EmployeeId,
                BaseSalary = entity.BaseSalary,
                Bonus = entity.Bonus,
                Deductions = entity?.Deductions,
                NetSalary = entity.NetSalary,
                PaymentDate = entity.PaymentDate,
            };
            await _unitOfWorkRepository.payrollRepository.AddAsync(newPayroll);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(string id, PayrollDTOs entity)
        {
            var item = await _unitOfWorkRepository.payrollRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($" Payroll with id = {id} not found");
            }

            // Update properties with validation
            // item.CartID = string.IsNullOrWhiteSpace(entity.CartID) ? item.CartID : entity.CartID.Trim();
            item.EmployeeId= entity.EmployeeId;
            item.BaseSalary= entity.BaseSalary;
            item.Bonus= entity.Bonus;
            item.Deductions= entity?.Deductions;
            item.NetSalary= entity.NetSalary;
            item.PaymentDate= entity.PaymentDate;
           
            // Set the UpdateDate to the current date and time
            item.UpdatedBy = _userContextService.UserName;
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.payrollRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.payrollRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Payroll with id = {id} not found");
            }
            await _unitOfWorkRepository.payrollRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }
        public async Task<IEnumerable<PayrollDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.payrollRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<PayrollDTOs>(item));
            return result;
        }
        public async Task<PayrollDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.payrollRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Payroll with id = {id} not found");
            }
            var result = _mapper.Map<PayrollDTOs>(item);
            return result;
        }
    }
}
