using FluentValidation;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtoValidators
{
    public class LocationReqDtoValidator : AbstractValidator<LocationReqDto>
    {
        public LocationReqDtoValidator()
        {
            RuleFor(dto => dto.TextDescription)
                .NotNull().WithMessage("Описание местоположения не должно быть пустым.")
                .MaximumLength(200).WithMessage("Максимальная длина описания составляет 200 символов.");

            RuleFor(dto => dto.Coordinate)
                .Null().When(dto => string.IsNullOrWhiteSpace(dto.TextDescription))
                .SetValidator(new CoordinateValidator())
                .When(dto => !string.IsNullOrWhiteSpace(dto.TextDescription));
        }
    }
}
