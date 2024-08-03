using InventoryApi.DTOs;
using InventoryApi.Repository.Interfaces.IRoleMenuMappingRepository;
using InventoryApi.Services.Interfaces;

namespace InventoryApi.Services.Implementation
{
    public class RoleMenuService : IRoleMenuService
    {
        private readonly IRoleMenuMappingRepository _repository;

        public RoleMenuService(IRoleMenuMappingRepository repository)
        {
            _repository = repository;
        }

        public async Task<RoleMenuMapping> GetRoleMenuMappingAsync(Guid roleId)
        {
            return await _repository.GetByRoleIdAsync(roleId);
        }

        public async Task UpdateRoleMenuMappingAsync(RoleMenuMapping roleMenuMapping)
        {
            await _repository.UpdateAsync(roleMenuMapping);
        }
        public async Task<RoleMappings> GetMappingsByRoleIdAsync(Guid roleId)
        {
            return await _repository.GetMappingsByRoleIdAsync(roleId);
        }
    }
}
