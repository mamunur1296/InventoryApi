namespace InventoryUi.Services.Interface
{
    public interface IFileUploader
    {
        Task<string> ImgUploader(IFormFile file, string folderName);
        Task<bool> DeleteFile(string fileName, string folderName);
        Task<byte[]> ProcessImageToByteAsync(IFormFile file);

    }
}
