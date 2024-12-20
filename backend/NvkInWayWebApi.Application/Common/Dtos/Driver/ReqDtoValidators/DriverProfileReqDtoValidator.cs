using FluentValidation;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos;

namespace NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtoValidators
{
    public class DriverProfileReqDtoValidator : AbstractValidator<DriverProfileReqDto>
    {
        public DriverProfileReqDtoValidator()
        {
            RuleFor(dto => dto.TgProfileId)
                .GreaterThan(-1).WithMessage("ID профиля Telegram должен быть не отр");

            RuleForEach(dto => dto.Cars)
                .SetValidator(new CarReqDtoValidator());
        }
    }
}
