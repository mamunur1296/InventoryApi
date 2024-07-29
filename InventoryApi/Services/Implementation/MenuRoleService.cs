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

        public async Task<ResponseDTOs<string>> CreateAsync(MenuRoleDTOs entity)
        {
            var response = new ResponseDTOs<string>();
            var newMenuRole = new MenuRole
            {
                MenuId = Guid.NewGuid().ToString(),
                //Menu = entity.Menu,
                //Role= entity.Role,
                RoleId = entity.RoleId
            };
            await _unitOfWorkRepository.menuRoleRepository.AddAsync(newMenuRole);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<string>> DeleteAsync(string id)
        {
            var response = new ResponseDTOs<string>();
            var deleteItem = await _unitOfWorkRepository.menuRoleRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Menu role   with id = {id} not found");
            }
            await _unitOfWorkRepository.menuRoleRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<IEnumerable<MenuRoleDTOs>>> GetAllAsync()
        {
            var response = new ResponseDTOs<IEnumerable<MenuRoleDTOs>>();


            var companyList = await _unitOfWorkRepository.menuRoleRepository.GetAllAsync();
            var result = companyList.Select(x => new MenuRoleDTOs()
            {
                RoleId = x.RoleId,
               // Role = x.Role,
               // Menu = x.Menu,
                MenuId = x.MenuId   
            });
            response.Data = result;
            response.Success = true;
            return response;
        }

        public Task<ResponseDTOs<MenuRoleDTOs>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTOs<string>> UpdateAsync(MenuRoleDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
