namespace InventoryUi.Services.Interface
{
    public interface IImageProcessor<T> where T : class
    {
        Task ProcessImageAsync(IFormFile file, T model);

    }
}
