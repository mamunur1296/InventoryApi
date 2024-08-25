using InventoryUi.Services.Interface;
using System.IO;
using System.Reflection;
public class FileUploader : IFileUploader
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileUploader(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> ImgUploader(IFormFile file , string folderName)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is null or empty", nameof(file));
        }

        string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
        string folderPath = string.IsNullOrEmpty(folderName) ? Path.Combine(_webHostEnvironment.WebRootPath, "Images") : Path.Combine(_webHostEnvironment.WebRootPath, "Images", folderName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return fileName;
    }

    public async Task<bool> DeleteFile(string fileName, string folderName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("File name is null or empty", nameof(fileName));
        }

        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", folderName , fileName);

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) if necessary
                return false;
            }
        }
        return false;
    }

    public async Task<byte[]> ProcessImageToByteAsync(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                byte[] photoBytes = ms.ToArray(); // Save the photo as a byte array in the database

                // Convert the byte array to a Base64 string
                string base64String = Convert.ToBase64String(photoBytes);

                // Determine the image type from the file content type
                string imageType = file.ContentType; // For example, "image/png"

                // Set the PhotoPath to the Base64 data URI (this assumes you have a model object)
               // model.PhotoPath = $"data:{imageType};base64,{base64String}";

                return photoBytes; // Return the byte array
            }
        }

        return null; // Return null if the file is null or empty
    }

}
