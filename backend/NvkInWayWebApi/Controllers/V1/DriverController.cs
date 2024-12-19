using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos;
using NvkInWayWebApi.Application.Common.Dtos.Passenger.ResDtos;
using NvkInWayWebApi.Application.Interfaces;
using System.Net;

namespace NvkInWayWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService service;

        public DriverController(IDriverService service)
        {
            this.service = service;
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
        public async Task<ActionResult<DriverProfileResDto>> GetDriverProfileById(long profileId)
        {
            var result = await service.GetDriverProfileByIdAsync(profileId);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorText);

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
        public async Task<ActionResult> CreateDirverProfile([FromBody] DriverProfileReqDto driverProfileReq)
        {
            var addResult = await service.AddDriverProfileAsync(driverProfileReq);

            if (!addResult.IsSuccess)
                return BadRequest(addResult.ErrorText);

            return Created();
        }

        /// <summary>
        /// Deletes the user profile with the corresponding telegram ID
        /// </summary>
        /// <param name="profileId">telegram user ID</param>
        /// <returns></returns>
        [HttpDelete("del-profile/{profileId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<DriverProfileResDto>> DeleteDriverProfileById(long profileId)
        {
            var result = await service.DeleteDriverProfileByIdAsync(profileId);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorText);

            return NoContent();
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
        public async Task<ActionResult> UpdateDriverProfileCars([FromBody] List<DetailedСarReqDto> listDetailedCars,
            [FromQuery] long driverProfileId)
        {
            var result = await service.UpdateDriverCars(driverProfileId, listDetailedCars);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorText);

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
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> DeleteDriverProfileCars([FromBody] List<Guid> listCarIds,
            [FromQuery] long driverProfileId)
        {
            var result = await service.DeleteDriverCars(driverProfileId, listCarIds);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorText);

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
        public async Task<ActionResult> AddDriverProfileCars([FromBody] List<CarReqDto> listDetailedCars,
            [FromQuery] long driverProfileId)
        {
            var result = await service.AddDriverCars(driverProfileId, listDetailedCars);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorText);

            return Created();
        }

        #endregion

        /// <summary>
        /// Creates a new trip
        /// </summary>
        /// <param name="createReqDto">Trip data</param>
        /// <returns></returns>
        [HttpPost("create-trip")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateTrip([FromBody] CreateTripReqDto createReqDto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns all active trips for the driver
        /// </summary>
        /// <param name="profileId">telegram user ID</param>
        /// <returns></returns>
        [HttpGet("get-active-trip/{profileId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DriverProfileResDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetActiveTripsResDto>> GetActiveTripsForDriver(long profileId)
        {
            throw new NotImplementedException();
        }
    }
}
