using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos;

namespace NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtoValidators
{
    using FluentValidation;

    public class CarReqDtoValidator : AbstractValidator<CarReqDto>
    {
        public CarReqDtoValidator()
        {
            RuleFor(x => x.AutoNumber)
                .NotEmpty()
                    .WithMessage("Номер автомобиля не может быть пустым.")
                .Matches(@"^[A-Z]\d{3}[A-Z]{2}\d{3}$")
                    .WithMessage("Номер автомобиля должен соответствовать формату 'A99AA999'.")
                .Length(8, 9)
                    .WithMessage("Длина номера должна составлять от 8 до 9 символов.");
        }
    }
}
