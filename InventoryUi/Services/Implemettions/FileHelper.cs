using InventoryUi.Services.Interface;

namespace InventoryUi.Services.Implemettions
{
    public class FileHelper : IFileHelper
    {
        private readonly IWebHostEnvironment _environment;

        public FileHelper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<bool> FileExists(string path)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, path.TrimStart('/'));
            return System.IO.File.Exists(fullPath);
        }
    }
}
