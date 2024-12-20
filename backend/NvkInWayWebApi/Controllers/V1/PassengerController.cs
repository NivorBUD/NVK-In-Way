using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NvkInWayWebApi.Application.Common;
using NvkInWayWebApi.Application.Common.Dtos.Passenger.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.Passenger.ResDtos;
using NvkInWayWebApi.Application.Interfaces;

namespace NvkInWayWebApi.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService service;

        public PassengerController(IPassengerService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets the passenger's profile by the telegram user ID
        /// </summary>
        /// <param name="profileId">telegram user ID</param>
        /// <returns></returns>
        [HttpGet("get-passenger-profile/{profileId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PassengerShortResDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PassengerShortResDto>> GetPassengerProfileById(long profileId)
        {
            var result = await service.GetPassengerProfileByIdAsync(profileId);

            if (!result.IsSuccess)
                return BadRequest(new MyResponseMessage(result.ErrorText));

            return Ok(result.Data);
        }

        /// <summary>
        /// Create new user profile
        /// </summary>
        /// <param name="passengerProfileReq">Create new passenger with data</param>
        /// <returns></returns>
        [HttpPost("create-passenger-profile")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreatePassengerProfile([FromBody] PassengerShortProfileReqDto passengerProfileReq)
        {
            var profileExsist = await service.GetPassengerProfileByIdAsync(passengerProfileReq.TgProfileId);

            if (profileExsist.IsSuccess)
                return BadRequest("Профиль уже существует");

            var addResult = await service.AddPassengerProfileAsync(passengerProfileReq);

            if (!addResult.IsSuccess)
                return BadRequest(new MyResponseMessage(addResult.ErrorText));

            return Created();
        }

        /// <summary>
        /// Deletes the user's profile
        /// </summary>
        /// <param name="passengerProfileReq"></param>
        /// <returns></returns>
        [HttpDelete("delete-passenger-profile")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeletePassengerProfile([FromBody] PassengerShortProfileReqDto passengerProfileReq)
        {
            var deleteResult = await service.DeletePassengerProfileAsync(passengerProfileReq.TgProfileId);

            if (!deleteResult.IsSuccess)
                return BadRequest(new MyResponseMessage(deleteResult.ErrorText));

            return NoContent();
        }

        /// <summary>
        /// Updates passenger profile data
        /// </summary>
        /// <param name="passengerProfileReq"></param>
        /// <returns></returns>
        [HttpPatch("update-passenger-rating")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdatePassengerRating([FromBody] PassengerFullProfileReqDto passengerProfileReq)
        {
            var updateResult = await service.UpdatePassengerProfileAsync(passengerProfileReq);

            if (!updateResult.IsSuccess)
                return BadRequest(new MyResponseMessage(updateResult.ErrorText));

            return Created();
        }
    }
}
