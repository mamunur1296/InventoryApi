using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class MenuService : IBaseServices<MenuDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public MenuService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(MenuDTOs entity)
        {
            var newMenu = new Menu
            {
                Id = Guid.NewGuid().ToString(),
                Name = entity.Name.Trim(),
                Url = entity.Url?.Trim(),
            };
            await _unitOfWorkRepository.menuRepository.AddAsync(newMenu);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.menuRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Menu   with id = {id} not found");
            }
            await _unitOfWorkRepository.menuRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<MenuDTOs>> GetAllAsync()
        {
            var MenuList = await _unitOfWorkRepository.menuRepository.GetAllAsync();
            var result = MenuList.Select(x => new MenuDTOs()
            {
                id = x.Id,
                Url = x.Url,
                Name = x.Name
            });
            return result;
        }

        public async Task<MenuDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.menuRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Customer with id = {id} not found");
            }
            var result = _mapper.Map<MenuDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, MenuDTOs entity)
        {
            var item = await _unitOfWorkRepository.menuRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"company with id = {id} not found");
            }
            // Update  properties
            item.Name = string.IsNullOrWhiteSpace(entity.Name) ? item.Name : entity.Name;
            item.Url = string.IsNullOrWhiteSpace(entity.Url) ? item.Url : entity.Url;
            
            // Perform update operation
            await _unitOfWorkRepository.menuRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }
    }
}
