using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebAppSummerSchool.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, IWebHostEnvironment env);
    }
}