using InventoryApi.DataContext;
using InventoryApi.DTOs;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services.Implementation
{
    public class DeliveryAddressServices : IDeliveryAddressServices
    {
        private readonly ApplicationDbContext _context;

        public DeliveryAddressServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeliveryAddressDTOs> GetByUserIdAsync(string userId)
        {
            // Validate userId
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");
            }

            // Fetch the first delivery address for the user where UserId matches the provided userId
            var address = await _context.deliveryAddresses
                .Where(da => da.UserId == userId)
                .Select(da => new DeliveryAddressDTOs
                {
                    Id = da.Id,
                    UserId = da.UserId,
                    Phone = da.Phone,
                    Address = da.Address,
                    Mobile= da.Mobile,
                    IsDefault = da.IsDefault
                }).FirstOrDefaultAsync(); // Retrieve the first or default address

            // Check if address is found
            if (address == null)
            {
                throw new NotFoundException($"No delivery address found for user with ID: {userId}");
            }

            return address;
        }


    }
}
