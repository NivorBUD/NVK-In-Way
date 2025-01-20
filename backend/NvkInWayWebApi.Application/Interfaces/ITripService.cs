using NvkInWayWebApi.Application.Common.Dtos.CarTrip.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.CarTrip.ResDtos;
using NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos;
using NvkInWayWebApi.Domain;

namespace NvkInWayWebApi.Application.Interfaces
{
    public interface ITripService
    {
        public Task<OperationResult<List<ShortActiveTripResDto>>> GetShortTripInfoByIntervalAsync(
            IntervalSearchReqDto searchDto, int startIndex, int count);

        public Task<OperationResult<List<GetActiveTripsResDto>>> GetTripsByDriverIdAsync(long driverId);

        public Task<OperationResult<List<GetActiveTripsResDto>>> GetTripsByPassengerIdAsync(long passengerId);

        public Task<OperationResult<GetActiveTripsResDto>> GetTripById(Guid id);

        public Task<OperationResult> AddDriverTripAsync(CreateTripReqDto tripReqDto);

        public Task<OperationResult> UpdateDriverTripAsync(CreateTripReqDto tripReqDto);

        public Task<OperationResult> DeleteTripAsync(Guid trip);

        public Task<OperationResult> RecordToTrip(RecordReqDto recordReqDto);

        public Task<OperationResult> CompleteTripAsync(EndTripReqDto endTripReqDto);

        public Task<OperationResult> RateParticipantAsync(SetRatingReqDto setRatingReqDto);
    }
}
