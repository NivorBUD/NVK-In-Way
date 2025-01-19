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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Tga;

namespace NvkInWayWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService service;

        private readonly IWebHostEnvironment webHostEnvironment;

        private readonly ImageService imageService;

        public DriverController(IDriverService service, IWebHostEnvironment webHostEnvironment, ImageService imageService)
        {
            this.service = service;
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
            var result = await service.GetDriverProfileByIdAsync(profileId);

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
            var addResult = await service.AddDriverProfileAsync(driverProfileReq);

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
            var result = await service.UpdateDriverCars(driverProfileId, listDetailedCars);

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
            var result = await service.DeleteDriverCars(driverProfileId, listCarIds);

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
            var result = await service.AddDriverCars(driverProfileId, listDetailedCars);

            if (!result.IsSuccess)
                return BadRequest(new MyResponseMessage(result.ErrorText));

            return Created();
        }

        [HttpPost("upload-car-image")]
        public async Task<ActionResult> UploadCarImage(IFormFile file, [FromForm] Guid carId)
        {
            try
            {
                if (file.Length > 0)
                {
                    var path = webHostEnvironment.WebRootPath;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var saveResult = await imageService.SaveImageAsync(
                        file, TgaImageType.RleColorMapped, ImageService.CreateSavePathFromGuid(carId));

                    if (!saveResult.IsSuccess)
                        return StatusCode(saveResult.StatusCode, new MyResponseMessage(saveResult.ErrorText));

                    return Ok(new MyResponseMessage("Изображение успешно сохранено"));
                }

                return BadRequest(new MyResponseMessage("Файл не был передан"));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new MyResponseMessage(ex.Message));
            }
        }

        [HttpGet("download-car-image/{fileName}")]
        public async Task<IActionResult> Get([FromRoute] string fileName)
        {
            string path = webHostEnvironment.WebRootPath + "\\uploads\\";
            var filePath = path + fileName;
            if (System.IO.File.Exists(filePath))
            {
                byte[] b = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(b, "image/png");
            }

            return null;
        }

        #endregion
    }
}
