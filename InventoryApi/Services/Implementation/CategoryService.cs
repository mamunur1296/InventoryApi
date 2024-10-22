using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class CategoryService : IBaseServices<CategoryDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public CategoryService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<bool> CreateAsync(CategoryDTOs entity)
        {
            var newCategory = new Category
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = _userContextService.UserName,
                CreationDate = DateTime.Now, // Set CreationDate here
                CategoryName = entity.CategoryName.Trim(),
                Description = entity?.Description?.Trim(),
                ParentCategoryID= entity?.ParentCategoryID?.Trim(),
            };
            await _unitOfWorkRepository.categoryRepository.AddAsync(newCategory);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.categoryRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Catagory with id = {id} not found");
            }
            await _unitOfWorkRepository.categoryRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryDTOs>> GetAllAsync()
        {
            var CatagoryList = await _unitOfWorkRepository.categoryRepository.GetAllAsync();
            var result = CatagoryList.Select(item => _mapper.Map<CategoryDTOs>(item));
            return result;
        }

        public async Task<CategoryDTOs> GetByIdAsync(string id)
        {
            var catagory = await _unitOfWorkRepository.categoryRepository.GetByIdAsync(id);
            if (catagory == null || catagory?.Id != id)
            {
                throw new NotFoundException($"catagory with id = {catagory?.Id} not found");
            }
            var result =  _mapper.Map<CategoryDTOs>(catagory);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, CategoryDTOs entity)
        {
            var item = await _unitOfWorkRepository.categoryRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id )
            {
                throw new NotFoundException($"Catagory with id = {id} not found");
            }
            // Update  properties
            item.CategoryName = entity.CategoryName;
            item.Description = entity?.Description;
            item.ParentCategoryID = entity?.ParentCategoryID;

            // Set the UpdateDate to the current date and time
            item.UpdatedBy = _userContextService.UserName;
            item.SetUpdateDate(DateTime.Now);
            // Perform update operation
            await _unitOfWorkRepository.categoryRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }
    }
}
