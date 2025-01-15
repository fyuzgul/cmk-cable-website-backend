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

        public async Task<DeletionResult> DestoryImage(string imageUrl)
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
            return uploadResult.Url.ToString();
        }
        public async Task<string> UploadPdf(IFormFile fromFile, string folderName)
        {
            var uploadResult = new ImageUploadResult();
            using (var stream = fromFile.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(fromFile.FileName, stream),
                    Folder = folderName,
                    Transformation = new Transformation().Quality("100").FetchFormat("auto"),
                    Format = "pdf"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult.Url.ToString();
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
