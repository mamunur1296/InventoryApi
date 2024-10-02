using InventoryApi.DTOs;

namespace InventoryApi.Services.Interfaces
{
    public interface IPurchaseServices
    {
        public Task<bool> PurchaseProduct(PurchaseItemDTOs entitys);
    }
}
