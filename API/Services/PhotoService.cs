using System;
using API.Interfaces;
using CloudinaryDotNet;
using System.Threading.Tasks;//Task
using CloudinaryDotNet.Actions;//ImageUploadResult
using Microsoft.AspNetCore.Http;//IFormFile
using Microsoft.Extensions.Options;
using API.Helpers;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace API.Services
{
	public class PhotoService : IPhotoService
	{
		private readonly Cloudinary _cloudinary;

		public PhotoService(IOptions<CloudinarySettings> config)
        {
			var acc = new Account(
				config.Value.CloudName,
				config.Value.ApiKey,
				config.Value.ApiSecret
			);

			_cloudinary = new Cloudinary(acc);
			//_cloudinary.Api.Secure = true;
		}

		public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file) 
		{
			var uploadResult = new ImageUploadResult();
			if ( file.Length > 0)
            {
				using var stream = file.OpenReadStream();
				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(file.FileName, stream),
					Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
				};
				uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }
			return uploadResult;
		}

		public async Task<DeletionResult> DeletePhotoAsync(string publicId)
		{
			var deleteParams = new DeletionParams(publicId);

			var result = await _cloudinary.DestroyAsync(deleteParams);

			return result;
		}

	}
}
