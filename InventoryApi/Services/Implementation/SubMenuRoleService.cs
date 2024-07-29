using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class SubMenuRoleService : IBaseServices<SubMenuRoleDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public SubMenuRoleService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<ResponseDTOs<string>> CreateAsync(SubMenuRoleDTOs entity)
        {
            var response = new ResponseDTOs<string>();
            var newWarehouse = new SubMenuRole
            {
               SubMenuId = Guid.NewGuid().ToString(),
               RoleId = entity.RoleId,  
              // Role= entity.Role,
              // SubMenu=entity.SubMenu,

            };
            await _unitOfWorkRepository.subMenuRoleRepository.AddAsync(newWarehouse);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<string>> DeleteAsync(string id)
        {
            var response = new ResponseDTOs<string>();
            var deleteItem = await _unitOfWorkRepository.subMenuRoleRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Sub Menu Role  with id = {id} not found");
            }
            await _unitOfWorkRepository.subMenuRoleRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<IEnumerable<SubMenuRoleDTOs>>> GetAllAsync()
        {
            var response = new ResponseDTOs<IEnumerable<SubMenuRoleDTOs>>();


            var SubMenuRoleList = await _unitOfWorkRepository.subMenuRoleRepository.GetAllAsync();
            var result = SubMenuRoleList.Select(x => new SubMenuRoleDTOs()
            {
                RoleId = x.RoleId,
                //Role=x.Role,
                //SubMenu=x.SubMenu,
                SubMenuId = x.SubMenuId 
            });
            response.Data = result;
            response.Success = true;
            return response;
        }

        public Task<ResponseDTOs<SubMenuRoleDTOs>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTOs<string>> UpdateAsync(SubMenuRoleDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
