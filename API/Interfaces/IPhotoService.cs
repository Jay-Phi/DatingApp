using System.Threading.Tasks;//Task
using CloudinaryDotNet.Actions;//ImageUploadResult
using Microsoft.AspNetCore.Http;//IFormFile

namespace API.Interfaces
{
    public interface IPhotoService
    {
        public Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        public Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}