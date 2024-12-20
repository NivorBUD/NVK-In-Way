using NvkInWayWebApi.Application.Common.Dtos.Passenger.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.Passenger.ResDtos;
using NvkInWayWebApi.Domain;

namespace NvkInWayWebApi.Application.Interfaces
{
    public interface IPassengerService
    {
        public Task<OperationResult<PassengerShortResDto>> GetPassengerProfileByIdAsync(long profileId);

        public Task<OperationResult> AddPassengerProfileAsync(PassengerShortProfileReqDto passengerProfileReqDto);

        public Task<OperationResult> UpdatePassengerProfileAsync(PassengerFullProfileReqDto passengerFullProfileReqDto);

        public Task<OperationResult> DeletePassengerProfileAsync(long profileId);
    }
}
