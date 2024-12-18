using NvkInWayWebApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Application.Common.Dtos.Driver.ResDtos
{
    public class LocationResDto
    {
        public string TextDescription { get; set; }

        public Coordinate Coordinate { get; set; }
    }
}
