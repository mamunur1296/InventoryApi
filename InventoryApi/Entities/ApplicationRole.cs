using Microsoft.AspNetCore.Identity;

namespace InventoryApi.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { } // Parameterless constructor
        public ApplicationRole(string roleName) : base(roleName) { }
    }
}
