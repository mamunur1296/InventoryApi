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

        public MenuService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<bool> CreateAsync(MenuDTOs entity)
        {
            var newMenu = new Menu
            {
                Id = Guid.NewGuid().ToString(),
                Name = entity.Name,
                Url = entity.Url,
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

        public Task<MenuDTOs> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(MenuDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
