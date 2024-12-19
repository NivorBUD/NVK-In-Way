using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtos;

namespace NvkInWayWebApi.Application.Common.Dtos.Driver.ReqDtoValidators
{
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

            RuleFor(c => c.AutoColor)
                .NotEmpty()
                .WithMessage("Поле цвета автомобиля не должно быть пустым")
                .MaximumLength(20)
                .WithMessage("Длина поля цвет машины не должно быть больше 20 символов");

            RuleFor(c => c.AutoName)
                .NotEmpty()
                .WithMessage("Поле название автомобиля не должно быть пустым")
                .MaximumLength(40)
                .WithMessage("Длина поля название машины не должно быть больше 40 символов");
        }
    }
}
