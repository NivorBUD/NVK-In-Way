using NvkInWayWebApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos
{
    public class LocationResDto
    {
        public Guid Id { get; set; }

        public string TextDescription { get; set; }

        public Coordinate Coordinate { get; set; }
    }
}
