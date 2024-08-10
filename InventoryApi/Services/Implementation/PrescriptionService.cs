using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class PrescriptionService : IBaseServices<PrescriptionDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public PrescriptionService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(PrescriptionDTOs entity)
        {
            var newPrescription = new Prescription
            {
                Id = Guid.NewGuid().ToString(),
                CustomerID = entity.CustomerID.Trim(),
                DoctorName = entity.DoctorName.Trim(),
                PrescriptionDate = DateTime.Now,
                MedicationDetails = entity.MedicationDetails.Trim(),
                DosageInstructions = entity.DosageInstructions.Trim(),
            };
            await _unitOfWorkRepository.prescriptionRepository.AddAsync(newPrescription);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.prescriptionRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            await _unitOfWorkRepository.prescriptionRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<PrescriptionDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.prescriptionRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<PrescriptionDTOs>(item));
            return result;
        }

        public async Task<PrescriptionDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.prescriptionRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            var result = _mapper.Map<PrescriptionDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, PrescriptionDTOs entity)
        {
            var item = await _unitOfWorkRepository.prescriptionRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Prescription with id = {id} not found");
            }

            // Update properties with validation and trimming
            item.CustomerID = string.IsNullOrWhiteSpace(entity.CustomerID) ? item.CustomerID : entity.CustomerID.Trim();
            item.DoctorName = string.IsNullOrWhiteSpace(entity.DoctorName) ? item.DoctorName : entity.DoctorName.Trim();
            item.PrescriptionDate = entity.PrescriptionDate ; // Assuming PrescriptionDate can be nullable
            item.MedicationDetails = string.IsNullOrWhiteSpace(entity.MedicationDetails) ? item.MedicationDetails : entity.MedicationDetails.Trim();
            item.DosageInstructions = string.IsNullOrWhiteSpace(entity.DosageInstructions) ? item.DosageInstructions : entity.DosageInstructions.Trim();

            // Perform update operation
            await _unitOfWorkRepository.prescriptionRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
