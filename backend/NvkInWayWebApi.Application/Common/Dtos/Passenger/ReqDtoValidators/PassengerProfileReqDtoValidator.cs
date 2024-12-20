using FluentValidation;
using NvkInWayWebApi.Application.Common.Dtos.Passenger.ReqDtos;

namespace NvkInWayWebApi.Application.Common.Dtos.Passenger.ReqDtoValidators
{
    public class PassengerProfileReqDtoValidator : AbstractValidator<PassengerShortProfileReqDto>
    {
        public PassengerProfileReqDtoValidator()
        {
            RuleFor(dto => dto.TgProfileId)
                .GreaterThan(-1).WithMessage("ID профиля Telegram должен быть не отр");
        }
    }
}
