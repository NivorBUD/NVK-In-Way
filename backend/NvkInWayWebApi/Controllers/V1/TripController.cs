using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NvkInWayWebApi.Application.Common.Dtos.CarTrip;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos;
using NvkInWayWebApi.Application.Common.Dtos.Passenger.ResDtos;
using NvkInWayWebApi.Application.Interfaces;

namespace NvkInWayWebApi.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class TripController : ControllerBase
    {
        private readonly ITripService service;

        public TripController(ITripService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Returns all active trips for the driver
        /// </summary>
        /// <param name="profileId">telegram user ID</param>
        /// <returns></returns>
        [HttpGet("get-driver-trips/{driverId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetActiveTripsResDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetActiveTripsResDto>> GetActiveTripsForDriver(long driverId)
        {
            var result = await service.GetTripsByDriverIdAsync(driverId);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorText);

            return Ok(result.Data);
        }

        /// <summary>
        /// Creates a new trip
        /// </summary>
        /// <param name="createReqDto">Trip data</param>
        /// <returns></returns>
        [HttpPost("create-trip")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateTrip([FromBody] CreateTripReqDto createReqDto)
        {
            var addResult = await service.AddDriverTripAsync(createReqDto);

            if (!addResult.IsSuccess)
                return BadRequest(addResult.ErrorText);

            return Created();
        }

        /// <summary>
        /// Returns brief information about the passengers of the trip
        /// </summary>
        /// <param name="tripId">The id of Trip</param>
        /// <returns></returns>
        [HttpGet("get-trip-passengers/{tripId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DriverProfileResDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PassengerShortResDto>> GetShortInfoTripPassengers(Guid tripId)
        {
            var result = await service.GetTripById(tripId);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorText);

            return Ok(result.Data);
        }
    }
}
