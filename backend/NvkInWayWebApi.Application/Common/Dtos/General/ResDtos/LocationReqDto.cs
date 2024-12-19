using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos
{
    public class LocationReqDto
    {
        public string TextDescription { get; set; }

        public Coordinate Coordinate { get; set; }
    }
}
