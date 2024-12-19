using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<PassengerShortResDto>> GetPassengerProfileById(long profileId)
        {
            var result = await service.GetPassengerProfileByIdAsync(profileId);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorText);

            return Ok(result.Data);
        }

        [HttpPost("create-passenger-profile")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreatePassengerProfile([FromBody] PassengerProfileReqDto passengerProfileReq)
        {
            var profileExsist = await service.GetPassengerProfileByIdAsync(passengerProfileReq.TgProfileId);

            if (profileExsist.IsSuccess)
                return BadRequest("Профиль уже существует");

            var addResult = await service.AddPassengerProfileAsync(passengerProfileReq);

            if (!addResult.IsSuccess)
                return BadRequest(addResult.ErrorText);

            return Created();
        }
    }
}
