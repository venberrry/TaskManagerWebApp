using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace WebAppSummerSchool.Services
{
    public class FileService : IFileService
    {
        public async Task<string> UploadFileAsync(IFormFile file, IWebHostEnvironment env)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            
            var fileName = Path.GetFileName(file.FileName);
            var relativePath = Path.Combine("/Assets/UserPhotos", fileName); // Путь для сохранения в базу данных
            var fullPath = Path.Combine(env.WebRootPath, "Assets", "UserPhotos", fileName); // Абсолютный путь

            // Создание папки, если она не существует
            var directory = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return relativePath; // Возвращаем относительный путь для хранения в базе данных
        }
    }
}