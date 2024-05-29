using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public class FileService
{
    private readonly string _targetFilePath;

    public FileService()
    {
        _targetFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file.Length > 0)
        {
            var filePath = Path.Combine(_targetFilePath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
        return null;
    }
}
