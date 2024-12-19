using FluentValidation;
using NvkInWayWebApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Application.Common.Dtos.General.ReqDtosValidators
{
    public class CoordinateValidator : AbstractValidator<Coordinate>
    {
        public CoordinateValidator()
        {
            RuleFor(coord => coord.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Широта должна находиться в диапазоне от -90 до 90 градусов.");

            RuleFor(coord => coord.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Долгота должна находиться в диапазоне от -180 до 180 градусов.");
        }
    }
}
