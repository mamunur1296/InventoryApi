using InventoryApi.DataContext;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces.IRoleMenuMappingRepository;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Repository.Implementation.RoleMenuMappingRepository
{
    public class RoleMenuMappingRepository : IRoleMenuMappingRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleMenuMappingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RoleMenuMapping> GetByRoleIdAsync(Guid roleId)
        {
            var menuRoles = await _context.MenuRoles.Where(mr => mr.RoleId == roleId.ToString()).ToListAsync();
            var menuIds = menuRoles.Select(mr => mr.MenuId).ToList();
            return new RoleMenuMapping { RoleId = roleId.ToString(), MenuIds = menuIds };
        }

        public async Task UpdateAsync(RoleMenuMapping roleMenuMapping)
        {
            var existingMenuRoles = await _context.MenuRoles.Where(mr => mr.RoleId == roleMenuMapping.RoleId.ToString()).ToListAsync();
            _context.MenuRoles.RemoveRange(existingMenuRoles);

            var newMenuRoles = roleMenuMapping.MenuIds.Select(menuId => new MenuRole
            {
                MenuId = menuId,
                RoleId = roleMenuMapping.RoleId.ToString()
            }).ToList();

            _context.MenuRoles.AddRange(newMenuRoles);
            await _context.SaveChangesAsync();
        }
        public async Task<RoleMappings> GetMappingsByRoleIdAsync(Guid roleId)
        {
            var roleStringId = roleId.ToString();

            // Get RoleMenuMapping
            var menuRoles = await _context.MenuRoles.Where(mr => mr.RoleId == roleStringId).ToListAsync();
            var menuIds = menuRoles.Select(mr => mr.MenuId).ToList();
            var roleMenuMapping = new RoleMenuMapping { RoleId = roleStringId, MenuIds = menuIds };

            // Get MenuSubmenuMappings
            var menuSubmenuMappings = new List<MenuSubmenuMapping>();
            foreach (var menuId in menuIds)
            {
                var subMenuIds = await _context.SubMenus.Where(sm => sm.MenuId == menuId).Select(sm => sm.Id).ToListAsync();
                menuSubmenuMappings.Add(new MenuSubmenuMapping { MenuId = menuId, SubMenuIds = subMenuIds });
            }

            // Get SubmenuActionMappings
            var submenuActionMappings = new List<SubmenuActionMapping>();
            foreach (var menuSubmenuMapping in menuSubmenuMappings)
            {
                foreach (var subMenuId in menuSubmenuMapping.SubMenuIds)
                {
                    var actionRoles = await _context.ActionRoles.Where(ar => ar.SubMenuId == subMenuId).ToListAsync();
                    var actionNameIds = actionRoles.Select(ar => ar.ActionId).ToList();
                    submenuActionMappings.Add(new SubmenuActionMapping { SubMenuId = subMenuId, ActionNameIds = actionNameIds });
                }
            }

            return new RoleMappings
            {
                RoleMenuMapping = roleMenuMapping,
                MenuSubmenuMappings = menuSubmenuMappings,
                SubmenuActionMappings = submenuActionMappings
            };
        }
    }
}
