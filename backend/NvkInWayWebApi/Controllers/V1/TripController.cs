using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NvkInWayWebApi.Application.Common;
using NvkInWayWebApi.Application.Common.Dtos.CarTrip.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.CarTrip.ResDtos;
using NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos;
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
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetActiveTripsResDto>> GetActiveTripsForDriver(long driverId)
        {
            var result = await service.GetTripsByDriverIdAsync(driverId);

            if (!result.IsSuccess)
                return BadRequest(new MyResponseMessage(result.ErrorText));

            return Ok(result.Data);
        }

        /// <summary>
        /// Returns all active trips for the passenger
        /// </summary>
        /// <param name="passengerId">telegram user ID</param>
        /// <returns></returns>
        [HttpGet("get-passenger-trips/{passengerId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetActiveTripsResDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetActiveTripsResDto>> GetActiveTripsForPassenger(long passengerId)
        {
            var result = await service.GetTripsByPassengerIdAsync(passengerId);

            if (!result.IsSuccess)
                return BadRequest(new MyResponseMessage(result.ErrorText));

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
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateTrip([FromBody] CreateTripReqDto createReqDto)
        {
            var addResult = await service.AddDriverTripAsync(createReqDto);

            if (!addResult.IsSuccess)
                return BadRequest(new MyResponseMessage(addResult.ErrorText));

            return Created();
        }

        /// <summary>
        /// Returns brief information about the passengers of the trip
        /// </summary>
        /// <param name="tripId">The id of Trip</param>
        /// <returns></returns>
        [HttpGet("get-trip-info/{tripId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetActiveTripsResDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetActiveTripsResDto>> GetShortInfoTripPassengers(Guid tripId)
        {
            var result = await service.GetTripById(tripId);

            if (!result.IsSuccess)
                return BadRequest(new MyResponseMessage(result.ErrorText));

            return Ok(result.Data);
        }


        /// <summary>
        /// Returns brief information about existing trips found by the query settings
        /// </summary>
        /// <param name="interval">Interval settings</param>
        /// <param name="startIndex">start index</param>
        /// <param name="count">trips get count</param>
        /// <returns></returns>
        [HttpPost("search-trips")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ShortActiveTripResDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ShortActiveTripResDto>>> GetShortTripInfoByInterval(
            [FromBody] IntervalSearchReqDto interval, int startIndex, int count)
        {
            var result = await service.GetShortTripInfoByIntervalAsync(interval, startIndex, count);

            if (!result.IsSuccess)
                return BadRequest(new MyResponseMessage(result.ErrorText));

            return Ok(result.Data);
        }


        /// <summary>
        /// sign up for a trip
        /// </summary>
        /// <param name="recordReqDto">Record to trip settings</param>
        /// <returns></returns>
        [HttpPost("record-to-trip")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RecordToTrip([FromBody] RecordReqDto recordReqDto)
        {
            var addResult = await service.RecordToTrip(recordReqDto);

            if (!addResult.IsSuccess)
                return BadRequest(new MyResponseMessage(addResult.ErrorText));

            return Created();
        }

        [HttpPost("complete-trip")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CompleteTrip(EndTripReqDto endTripReqDto)
        {
            var result = await service.CompleteTripAsync(endTripReqDto);

            if (!result.IsSuccess)
                return BadRequest(new MyResponseMessage(result.ErrorText));

            return Ok();
        }

        [HttpPost("rate-users")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RateParticipants([FromBody] SetRatingReqDto rating)
        {
            var result = await service.RateParticipantAsync(rating);

            if (!result.IsSuccess)
                return BadRequest(new MyResponseMessage(result.ErrorText));

            return Ok();
        }

    }
}
