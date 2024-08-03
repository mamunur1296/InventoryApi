using InventoryApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Services.Interfaces
{
    public interface IRoleMenuService
    {
        Task<RoleMenuMapping> GetRoleMenuMappingAsync(Guid roleId);
        Task UpdateRoleMenuMappingAsync(RoleMenuMapping roleMenuMapping);
        Task<RoleMappings> GetMappingsByRoleIdAsync(Guid roleId);
    }
}
