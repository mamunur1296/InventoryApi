using InventoryUi.Models;
using InventoryUi.Services.Interface;

namespace InventoryUi.Services.Implemettions
{
    public class ImageProcessor : IImageProcessor<Employee>
    {
        public async Task ProcessImageAsync(IFormFile file, Employee model)
        {
            if (file != null && file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    model.Photo = ms.ToArray(); // Save the photo as a byte array in the database
                    // Convert the byte array to a Base64 string
                    string base64String = Convert.ToBase64String(model.Photo);

                    // Determine the image type from the file content type
                    string imageType = file.ContentType; // For example, "image/png"

                    // Set the PhotoPath to the Base64 data URI
                    model.PhotoPath = $"data:{imageType};base64,{base64String}";
                }
            }
        }
    }
}
