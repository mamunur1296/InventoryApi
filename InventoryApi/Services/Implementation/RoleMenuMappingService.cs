using InventoryApi.DataContext;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services.Implementation
{
    public class RoleMenuMappingService
    {
        private readonly ApplicationDbContext _context;

        public RoleMenuMappingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task UpdateRoleMenuMappings(RoleMenuMappingDto dto)
        {
            // Validate input
            if (dto == null || string.IsNullOrEmpty(dto.RoleId) || dto.MenuIds == null || dto.SubMenuIds == null)
            {
                throw new ArgumentException("Invalid input data.");
            }

            try
            {
                // Log start of method
                Console.WriteLine("Starting UpdateRoleMenuMappings");

                // Remove existing mappings for the role
                var existingMenuRoles = _context.MenuRoles.Where(mr => mr.RoleId == dto.RoleId);
                var existingSubMenuRoles = _context.SubMenuRoles.Where(smr => smr.RoleId == dto.RoleId);

                _context.MenuRoles.RemoveRange(existingMenuRoles);
                _context.SubMenuRoles.RemoveRange(existingSubMenuRoles);
                await _context.SaveChangesAsync();

                // Log after removing old mappings
                Console.WriteLine("Removed existing mappings");

                // Add new menu roles
                foreach (var menuId in dto.MenuIds)
                {
                    Console.WriteLine($"Adding MenuRole: MenuId = {menuId}, RoleId = {dto.RoleId}");
                    _context.MenuRoles.Add(new MenuRole
                    {
                        MenuId = menuId, // Use MenuId as string
                        RoleId = dto.RoleId
                    });
                }

                // Add new submenu roles
                foreach (var subMenuId in dto.SubMenuIds)
                {
                    Console.WriteLine($"Adding SubMenuRole: SubMenuId = {subMenuId}, RoleId = {dto.RoleId}");
                    _context.SubMenuRoles.Add(new SubMenuRole
                    {
                        SubMenuId = subMenuId, // Use SubMenuId as string
                        RoleId = dto.RoleId
                    });
                }

                await _context.SaveChangesAsync();

                // Log after adding new mappings
                Console.WriteLine("Added new mappings successfully");
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in UpdateRoleMenuMappings: {ex.Message}");
                throw new Exception($"Internal server error: {ex.Message}", ex);
            }
        }



        public async Task<RoleMenuMappingDto> GetRoleMenuMappings(string roleId)
        {
            var menuIds = await _context.MenuRoles.Where(mr => mr.RoleId == roleId).Select(mr => mr.MenuId).ToListAsync();
            var subMenuIds = await _context.SubMenuRoles.Where(smr => smr.RoleId == roleId).Select(smr => smr.SubMenuId).ToListAsync();

            return new RoleMenuMappingDto
            {
                RoleId = roleId,
                MenuIds = menuIds,
                SubMenuIds = subMenuIds
            };
        }
    }
}
