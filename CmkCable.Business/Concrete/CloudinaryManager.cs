using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CmkCable.Entities;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.Business.Concrete
{
    public class CloudinaryManager
    {
        private Cloudinary _cloudinary;
        private CloudinaryInfo _cloudinaryInfo;
        public CloudinaryManager()
        {
            _cloudinaryInfo = new CloudinaryInfo();
            _cloudinary = new Cloudinary(_cloudinaryInfo.account);
        }

        public string GetCloudinaryPublicId(string imageUrl)
        {
            var uri = new Uri(imageUrl);

            var localPath = uri.LocalPath;

            var publicIdWithVersion = localPath.Substring(localPath.IndexOf("upload/") + 7);

            var publicId = publicIdWithVersion.Substring(publicIdWithVersion.IndexOf('/') + 1);

            publicId = publicId.Substring(0, publicId.LastIndexOf('.'));

            return publicId;
        }

        public async Task<DeletionResult> DestroyImage(string imageUrl)
        {
            var publicId = GetCloudinaryPublicId(imageUrl);

            if (string.IsNullOrEmpty(publicId))
            {
                Console.WriteLine("Public ID bulunamadı.");
                return null;
            }

            var deleteParams = new DeletionParams(publicId);
            var deletionResult = await _cloudinary.DestroyAsync(deleteParams);

            return deletionResult;
        }

        public async Task<string> UploadImage(IFormFile fromFile, string folderName)
        {
            try
            {
                var uploadResult = new ImageUploadResult();
                using (var stream = fromFile.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(fromFile.FileName, stream),
                        Transformation = new Transformation().Quality("100").FetchFormat("auto"),
                        Folder = folderName
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }

                // URL'yi kontrol et ve HTTP'yi HTTPS ile değiştir
                var url = uploadResult.Url.ToString();
                if (url.StartsWith("http://"))
                {
                    url = url.Replace("http://", "https://");
                }

                return url;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cloudinary image upload failed: {ex.Message}");
                // Fallback to base64
                using (var memoryStream = new MemoryStream())
                {
                    await fromFile.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    string base64Image = Convert.ToBase64String(imageBytes);
                    return $"data:{fromFile.ContentType};base64,{base64Image}";
                }
            }
        }
        public async Task<string> UploadPdf(IFormFile fromFile, string folderName)
        {
            try
            {
                var uploadResult = new RawUploadResult();
                using (var stream = fromFile.OpenReadStream())
                {
                    var uploadParams = new RawUploadParams()
                    {
                        File = new FileDescription(fromFile.FileName, stream),
                        Folder = folderName,
                        ResourceType = ResourceType.Raw
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }

                // URL'yi kontrol et ve HTTP'yi HTTPS ile değiştir
                var url = uploadResult.Url.ToString();
                if (url.StartsWith("http://"))
                {
                    url = url.Replace("http://", "https://");
                }

                return url;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cloudinary PDF upload failed: {ex.Message}");
                // Fallback to base64
                using (var memoryStream = new MemoryStream())
                {
                    await fromFile.CopyToAsync(memoryStream);
                    byte[] pdfBytes = memoryStream.ToArray();
                    string base64Pdf = Convert.ToBase64String(pdfBytes);
                    return base64Pdf;
                }
            }
        }


        public async Task<DeletionResult> DestroyPdf(string pdfUrl)
        {
            var publicId = GetCloudinaryPublicId(pdfUrl);

            if (string.IsNullOrEmpty(publicId))
            {
                Console.WriteLine("Public ID bulunamadı.");
                return null;
            }

            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };

            var deletionResult = await _cloudinary.DestroyAsync(deleteParams);

            return deletionResult;
        }


    }
}
