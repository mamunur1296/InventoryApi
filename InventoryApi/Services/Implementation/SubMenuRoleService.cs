using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public SubMenuRoleService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<bool> CreateAsync(SubMenuRoleDTOs entity)
        {
            var newWarehouse = new SubMenuRole
            {
               SubMenuId = Guid.NewGuid().ToString(),
               RoleId = entity.RoleId,  
              // Role= entity.Role,
              // SubMenu=entity.SubMenu,

            };
            await _unitOfWorkRepository.subMenuRoleRepository.AddAsync(newWarehouse);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.subMenuRoleRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Sub Menu Role  with id = {id} not found");
            }
            await _unitOfWorkRepository.subMenuRoleRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<SubMenuRoleDTOs>> GetAllAsync()
        {
            var SubMenuRoleList = await _unitOfWorkRepository.subMenuRoleRepository.GetAllAsync();
            var result = SubMenuRoleList.Select(x => new SubMenuRoleDTOs()
            {
                RoleId = x.RoleId,
                //Role=x.Role,
                //SubMenu=x.SubMenu,
                SubMenuId = x.SubMenuId 
            });
            return result;
        }

        public async Task<SubMenuRoleDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.subMenuRoleRepository.GetByIdAsync(id);
            if (item == null || item?.SubMenuId != id)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            var result = _mapper.Map<SubMenuRoleDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, SubMenuRoleDTOs entity)
        {
            var item = await _unitOfWorkRepository.subMenuRoleRepository.GetByIdAsync(id);
            if (item == null || item?.SubMenuId != id)
            {
                throw new NotFoundException($"company with id = {id} not found");
            }
            // Update  properties
            //item.Name = string.IsNullOrWhiteSpace(entity.Name) ? item.Name : entity.Name;

            // Perform update operation
            await _unitOfWorkRepository.subMenuRoleRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }
    }
}
