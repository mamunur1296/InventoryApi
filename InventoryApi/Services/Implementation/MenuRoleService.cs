using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class MenuRoleService  : IBaseServices<MenuRoleDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public MenuRoleService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<bool> CreateAsync(MenuRoleDTOs entity)
        {
            var newMenuRole = new MenuRole
            {
                MenuId = Guid.NewGuid().ToString(),
                //Menu = entity.Menu,
                //Role= entity.Role,
                RoleId = entity.RoleId
            };
            await _unitOfWorkRepository.menuRoleRepository.AddAsync(newMenuRole);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.menuRoleRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Menu role   with id = {id} not found");
            }
            await _unitOfWorkRepository.menuRoleRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<MenuRoleDTOs>> GetAllAsync()
        {
            var companyList = await _unitOfWorkRepository.menuRoleRepository.GetAllAsync();
            var result = companyList.Select(x => new MenuRoleDTOs()
            {
                RoleId = x.RoleId,
               // Role = x.Role,
               // Menu = x.Menu,
                MenuId = x.MenuId   
            });
            return result;
        }

        public Task<MenuRoleDTOs> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(MenuRoleDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
