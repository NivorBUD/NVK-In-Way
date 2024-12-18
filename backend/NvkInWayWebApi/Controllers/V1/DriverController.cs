using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos;

namespace NvkInWayWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class DriverController : ControllerBase
    {
        /// <summary>
        /// Gets the driver's profile by the telegram user ID
        /// </summary>
        /// <param name="profileId">telegram user ID</param>
        /// <returns></returns>
        [HttpGet("get-profile/{profileId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DriverProfileResDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<DriverProfileResDto>> GetDriverProfileById(long profileId)
        {
            throw new NotImplementedException();
        }

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
        [ProducesResponseType(typeof(DriverProfileResDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GetActiveTripsResDto>> GetActiveTripsForDriver(long profileId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns brief information about the passengers of the trip
        /// </summary>
        /// <param name="tripId">The id of Trip</param>
        /// <returns></returns>
        [HttpGet("get-trip-passengers/{tripId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DriverProfileResDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PassengerShortResDto>> GetShortInfoTripPassengers(Guid tripId)
        {
            throw new NotImplementedException();
        }
    }
}
