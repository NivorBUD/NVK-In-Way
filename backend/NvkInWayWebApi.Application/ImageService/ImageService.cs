using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NvkInWayWebApi.Application.Common;
using NvkInWayWebApi.Application.ImageService.Interfaces;
using NvkInWayWebApi.Domain;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Processing;

namespace NvkInWayWebApi.Application.ImageService
{
    public class ImageService
    {
        private readonly IEnumerable<IImageProfile> imageProfiles;

        private readonly IWebHostEnvironment webHostEnvironment;

        public ImageService(IEnumerable<IImageProfile> imageProfiles, IWebHostEnvironment webHostEnvironment)
        {
            this.imageProfiles = imageProfiles;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<OperationResult> SaveImageAsync(IFormFile formFile, TgaImageType imageType, string savePath)
        {
            var imageProfile = imageProfiles.FirstOrDefault(profile => 
                profile.ImageType == imageType);

            if(imageProfile == null) 
                return OperationResult.Error($"Профиль изображения не был найден. ");

            if(!IsValidExtension(formFile, imageProfile))
                return OperationResult.Error($"Не поддерживаемый тип файла изображения {Path.GetExtension(formFile.FileName)}");

            if(!IsValidFileSize(formFile, imageProfile))
                return OperationResult.Error(
                    $"Некорректный вес файла - максимальный размер {imageProfile.MaxSizeBytes % 1048576} МБайт");

            var image = await Image.LoadAsync(formFile.OpenReadStream());

            if(!IsValidImageSize(image, imageProfile))
                return OperationResult.Error(
                    $"Некорректный размер изображения - минимальный размер ({imageProfile.Width}, {imageProfile.Height})");

            var saveDestination = Path.Combine(webHostEnvironment.WebRootPath, imageProfile.Folder, savePath);
            var rootDir = Directory.GetParent(saveDestination).ToString();
            
            var checkDirResult = PrepareProfileFolder(rootDir);

            if(!checkDirResult.IsSuccess)
                return checkDirResult;

            Resize(image, imageProfile);
            await image.SaveAsJpegAsync(saveDestination + ".jpeg");

            return OperationResult.Success();
        }

        public async Task<OperationResult<DownloadData>> DownloadImageAsync(TgaImageType imageType, string path)
        {
            var imageProfile = imageProfiles.FirstOrDefault(profile =>
                profile.ImageType == imageType);

            if (imageProfile == null)
                return OperationResult<DownloadData>.Error($"Профиль изображения не был найден. ");

            var destinationPath = Path.Combine(webHostEnvironment.WebRootPath, imageProfile.Folder, path);

            if (!System.IO.File.Exists(destinationPath))
                return OperationResult<DownloadData>.Error("Изображение не найдено");

            var bytes = await System.IO.File.ReadAllBytesAsync(destinationPath);

            var name = Path.GetFileName(destinationPath);

            return OperationResult<DownloadData>.Success(new DownloadData(bytes, name, "image/jpeg"));
        }

        public async Task<OperationResult> DeleteImageAsync(string path, TgaImageType imageType)
        {
            var imageProfile = imageProfiles.FirstOrDefault(profile => 
                profile.ImageType == imageType);

            var destinationPath = Path.Combine(webHostEnvironment.WebRootPath, imageProfile.Folder, path);

            if (!System.IO.File.Exists(destinationPath))
                return OperationResult.Error($"Изображение [{path}] не найдено. ");

            System.IO.File.Delete(destinationPath);

            return OperationResult.Success();
        }

        private bool IsValidExtension(IFormFile formFile, IImageProfile imageProfile)
        {
            return imageProfile.AllowedExtensions
                .Contains(Path.GetExtension(formFile.FileName.ToLower()));
        }

        private bool IsValidFileSize(IFormFile formFile, IImageProfile imageProfile)
        {
            return formFile.Length < imageProfile.MaxSizeBytes;
        }

        private bool IsValidImageSize(Image image, IImageProfile imageProfile)
        {
            return image.Width > imageProfile.Width && image.Height > imageProfile.Height;
        }

        private void Resize(Image image, IImageProfile imageProfile)
        {
            var resizeOptions = new ResizeOptions
            {
                Mode = ResizeMode.Min,
                Size = new Size(imageProfile.Width, imageProfile.Height)
            };

            image.Mutate(action => action.Resize(resizeOptions));
        }

        private OperationResult PrepareProfileFolder(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                return OperationResult<string>.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex.Message, (int)HttpStatusCode.BadGateway);
            }
        }

        public static string CreateSavePathFromGuid(Guid guid)
        {
            var guidString = guid.ToString();
            var sb = new StringBuilder();

            sb.Append(guidString.Substring(0, 2));
            sb.Append("\\");
            sb.Append(guidString.Substring(2, 4));
            sb.Append("\\");
            sb.Append(guidString.Substring(6));

            return sb.ToString();
        }
    }

    public record DownloadData(byte[] Bytes, string FileName, string MimeType);
}
