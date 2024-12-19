using NvkInWayWebApi.Application.Common.Dtos.Passenger.ReqDtos;
using NvkInWayWebApi.Application.Common.Dtos.Passenger.ResDtos;
using NvkInWayWebApi.Application.Interfaces;
using NvkInWayWebApi.Domain;
using NvkInWayWebApi.Domain.Models.Profiles;
using NvkInWayWebApi.Domain.RepositoriesContract;

namespace NvkInWayWebApi.Application.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository repository;

        public PassengerService(IPassengerRepository passengerRepository)
        {
            repository = passengerRepository;
        }

        public async Task<OperationResult<PassengerShortResDto>> GetPassengerProfileByIdAsync(long profileId)
        {
            var passengerProfileResult = await repository.GetPassengerWithRecordsAsync(profileId);

            if (!passengerProfileResult.IsSuccess)
                return OperationResult<PassengerShortResDto>.Error(passengerProfileResult.ErrorText);

            var resDto = PassengerShortResDto.MapFrom(passengerProfileResult.Data);

            return OperationResult<PassengerShortResDto>.Success(resDto);
        }

        public async Task<OperationResult> AddPassengerProfileAsync(PassengerProfileReqDto passengerProfileReqDto)
        {
            var newProfile = new PassengerProfile()
            {
                TgProfileId = passengerProfileReqDto.TgProfileId
            };

            await repository.AddPassengerAsync(newProfile);
            return OperationResult.Success();
        }
    }
}
