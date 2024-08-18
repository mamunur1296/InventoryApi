using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class ReviewService : IBaseServices<ReviewDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(ReviewDTOs entity)
        {
            var newReview = new Review
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = entity.CreatedBy?.Trim(),
                CreationDate = DateTime.Now, // Set CreationDate here
                ProductID = entity.ProductID.Trim(),
                CustomerID = entity.CustomerID.Trim(),
                Rating = entity.Rating,
                ReviewDate = DateTime.Now,
                ReviewText = entity.ReviewText.Trim(),
            };
            await _unitOfWorkRepository.reviewRepository.AddAsync(newReview);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.reviewRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            await _unitOfWorkRepository.reviewRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<ReviewDTOs>> GetAllAsync()
        {
            var itemList = await _unitOfWorkRepository.reviewRepository.GetAllAsync();
            var result = itemList.Select(item => _mapper.Map<ReviewDTOs>(item));
            return result;
        }

        public async Task<ReviewDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.reviewRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            var result = _mapper.Map<ReviewDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, ReviewDTOs entity)
        {
            var item = await _unitOfWorkRepository.reviewRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Review with id = {id} not found");
            }

            // Update properties with validation and trimming
            item.ProductID = string.IsNullOrWhiteSpace(entity.ProductID) ? item.ProductID : entity.ProductID.Trim();
            item.CustomerID = string.IsNullOrWhiteSpace(entity.CustomerID) ? item.CustomerID : entity.CustomerID.Trim();
            item.Rating = entity?.Rating != null ? entity.Rating : item.Rating;
            item.ReviewText = string.IsNullOrWhiteSpace(entity?.ReviewText) ? item.ReviewText : entity.ReviewText.Trim();
            item.ReviewDate = DateTime.Now; // Update to the current date


            // Set the UpdateDate to the current date and time
            item.UpdatedBy = entity.UpdatedBy?.Trim();
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.reviewRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
