using InventoryApi.DTOs;

namespace InventoryApi.Repository.Interfaces.IRoleMenuMappingRepository
{
    public interface IRoleMenuMappingRepository
    {
        Task<RoleMenuMapping> GetByRoleIdAsync(Guid roleId);
        Task UpdateAsync(RoleMenuMapping roleMenuMapping);
        Task<RoleMappings> GetMappingsByRoleIdAsync(Guid roleId);
    }
}
