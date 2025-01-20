using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos;
using NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.Passenger.ResDtos;
using NvkInWayWebApi.Application.Interfaces;
using System.Net;
using NvkInWayWebApi.Application.Common;
using NvkInWayWebApi.Application.ImageService;
using NvkInWayWebApi.Domain;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Tga;
using NvkInWayWebApi.Domain.Models;
using System.IO;

namespace NvkInWayWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService dbService;

        private readonly IWebHostEnvironment webHostEnvironment;

        private readonly ImageService imageService;

        public DriverController(IDriverService dbService, IWebHostEnvironment webHostEnvironment, ImageService imageService)
        {
            this.dbService = dbService;
            this.webHostEnvironment = webHostEnvironment;
            this.imageService = imageService;
        }

        #region ProfileEndpoints

        /// <summary>
        /// Gets the driver's profile by the telegram user ID
        /// </summary>
        /// <param name="profileId">telegram user ID</param>
        /// <returns></returns>
        [HttpGet("get-profile/{profileId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DriverProfileResDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DriverProfileResDto>> GetDriverProfileById(long profileId)
        {
            var result = await dbService.GetDriverProfileByIdAsync(profileId);

            if (!result.IsSuccess)
                return BadRequest(new MyResponseMessage(result.ErrorText));

            return Ok(result.Data);
        }

        /// <summary>
        /// Creates a driver profile
        /// </summary>
        /// <param name="driverProfileReq">Driver profile dto</param>
        /// <returns></returns>
        [HttpPost("create-profile")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateDirverProfile([FromBody] DriverProfileReqDto driverProfileReq)
        {
            var addResult = await dbService.AddDriverProfileAsync(driverProfileReq);

            if (!addResult.IsSuccess)
                return BadRequest(new MyResponseMessage(addResult.ErrorText));

            return Created();
        }

        #endregion


        #region DriverCarsEndpoints

        /// <summary>
        /// Updates driver cars
        /// </summary>
        /// <param name="listDetailedCars">List of updated cars</param>
        /// <param name="driverProfileId">Driver telegram profile id</param>
        /// <returns></returns>
        [HttpPatch("update-driver-cars")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateDriverProfileCars([FromBody] List<DetailedСarReqDto> listDetailedCars,
            [FromQuery] long driverProfileId)
        {
            var result = await dbService.UpdateDriverCars(driverProfileId, listDetailedCars);

            if (!result.IsSuccess)
                return BadRequest(new MyResponseMessage(result.ErrorText));

            return Created();
        }

        /// <summary>
        /// Deletes driver cars
        /// </summary>
        /// <param name="listCarIds">The ids of cars to delete</param>
        /// <param name="driverProfileId">Driver telegram profile id</param>
        /// <returns></returns>
        [HttpDelete("delete-driver-cars")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteDriverProfileCars([FromBody] List<Guid> listCarIds,
            [FromQuery] long driverProfileId)
        {
            var carResults = await GetDictionaryCarGuidResultAsync(listCarIds);
            var errorCars = carResults
                .Where(kvp => !kvp.Value.IsSuccess)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ErrorText);

            if (errorCars.Any())
            {
                var errorMessage = string.Join(", ", errorCars.Select(kvp => $"ID: {kvp.Key}"));
                return BadRequest(new MyResponseMessage($"Машины не найдены: {errorMessage}"));
            }

            var imgDeleteResults = await DeleteCarsImagesAsync(carResults);

            if (imgDeleteResults.Any(r => !r.IsSuccess))
            {
                var errorMessage = string.Join(", ", imgDeleteResults
                    .Where(r => !r.IsSuccess)
                    .Select(r => r.ErrorText));

                return StatusCode((int)HttpStatusCode.BadRequest, new MyResponseMessage(errorMessage));
            }

            var result = await dbService.DeleteDriverCars(driverProfileId, listCarIds);

            if (!result.IsSuccess)
                return BadRequest(new MyResponseMessage(result.ErrorText));

            return Created();
        }

        /// <summary>
        /// Add new driver car
        /// </summary>
        /// <param name="listDetailedCars"></param>
        /// <param name="driverProfileId"></param>
        /// <returns></returns>
        [HttpPost("add-driver-cars")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddDriverProfileCars([FromBody] List<CarReqDto> listDetailedCars,
            [FromQuery] long driverProfileId)
        {
            var result = await dbService.AddDriverCars(driverProfileId, listDetailedCars);

            if (!result.IsSuccess)
                return BadRequest(new MyResponseMessage(result.ErrorText));

            return Created();
        }

        /// <summary>
        /// Upload an image of the driver's car
        /// </summary>
        /// <param name="file">The image file</param>
        /// <param name="carId">The car id</param>
        /// <returns></returns>
        [HttpPost("upload-car-image")]
        public async Task<ActionResult> UploadCarImage(IFormFile file, [FromForm] Guid carId)
        {
            try
            {
                if (!IsFileValid(file))
                {
                    return BadRequest(new MyResponseMessage("Файл не был передан"));
                }

                EnsureUploadsDirectoryExists();

                var setIsUploaded = await dbService.SetCarImageIsUploadedAsync(carId);

                if (!setIsUploaded.IsSuccess)
                    return StatusCode(setIsUploaded.StatusCode, setIsUploaded.ErrorText);

                var saveResult = await imageService.SaveImageAsync(
                    file, TgaImageType.RleColorMapped, ImageService.CreateSavePathFromGuid(carId));

                if (!saveResult.IsSuccess)
                {
                    await dbService.SetCarImageIsUnUploadedAsync(carId);
                    return StatusCode(saveResult.StatusCode, new MyResponseMessage(saveResult.ErrorText));
                }

                return Ok(new MyResponseMessage("Изображение успешно сохранено"));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new MyResponseMessage(ex.Message));
            }
        }

        /// <summary>
        /// Download an image of the driver's car, or return the standard image if not
        /// </summary>
        /// <param name="carId">The car id</param>
        /// <returns></returns>
        [HttpGet("download-car-image/{carId}")]
        public async Task<IActionResult> DownloadCarImage([FromRoute] Guid carId)
        {
            var carData = await dbService.GetCarByIdAsync(carId);
            if(!carData.IsSuccess)
                return StatusCode(carData.StatusCode, new MyResponseMessage(carData.ErrorText));

            var getPathResult = await GetDownloadCarPathAsync(carId);
            if(!getPathResult.IsSuccess)
                return StatusCode(getPathResult.StatusCode, new MyResponseMessage(getPathResult.ErrorText));

            var downloadPath = getPathResult.Data;

            var downloadData = await imageService.DownloadImageAsync(
                TgaImageType.RleColorMapped, downloadPath);

            if (!downloadData.IsSuccess)
                return StatusCode(downloadData.StatusCode, new MyResponseMessage(downloadData.ErrorText));
            
            return File(downloadData.Data.Bytes, downloadData.Data.MimeType, downloadData.Data.FileName);
        }

        #endregion

        #region Helpers

        private async Task<IEnumerable<OperationResult>> DeleteCarsImagesAsync(Dictionary<Guid, OperationResult<Car>> carIdResults)
        {
            var imgDelResults = new List<OperationResult>();

            foreach (var key in carIdResults.Keys)
            {
                imgDelResults.Append(await imageService.DeleteImageAsync(GetImagePathFromGuid(key), TgaImageType.RleColorMapped));
            }

            return imgDelResults;
        }

        private async Task<Dictionary<Guid, OperationResult<Car>>> GetDictionaryCarGuidResultAsync(IEnumerable<Guid> carIds)
        {
            var dict = new Dictionary<Guid, OperationResult<Car>>();

            foreach (var carId in carIds)
            {
                var result = await dbService.GetCarByIdAsync(carId);

                dict.Add(carId, result);
            }

            return dict;
        }

        private async Task<OperationResult<string>> GetDownloadCarPathAsync(Guid carId)
        {
            var car = await dbService.GetCarByIdAsync(carId);
            string path = string.Empty;

            if (!car.IsSuccess)
            {
                return OperationResult<string>.Error("Машина не найдена");
            }

            if (!car.Data.IsImageUploaded)
            {
                path = Path.Combine(webHostEnvironment.WebRootPath, "driver_cars", "anonim_vehicle.jpeg");
                return OperationResult<string>.Success(path);
            }

            path = GetImagePathFromGuid(carId);
            return OperationResult<string>.Success(path);
        }

        private string GetImagePathFromGuid(Guid guid)
        {
            return ImageService.CreateSavePathFromGuid(guid) + ".jpeg";
        }

        private bool IsFileValid(IFormFile file)
        {
            return file != null && file.Length > 0;
        }

        private void EnsureUploadsDirectoryExists()
        {
            var path = webHostEnvironment.WebRootPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        #endregion
    }
}
