using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Application.Common.Dtos.General.ReqDtos;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Common.Dtos.CarTrip.ReqDtos
{
    public class IntervalSearchReqDto
    {
        public LocationReqDto? StartPointAddress { get; set; }

        public LocationReqDto? EndPointAddress { get; set; }

        public DateTime? MaxEndTime { get; set; }

        public DateTime? MinDateTime { get; set; }

        public static TripSearchInterval MapFrom(IntervalSearchReqDto reqDto)
        {
            return new TripSearchInterval()
            {
                StartPointLocation = LocationReqDto.MapFrom(reqDto.StartPointAddress),
                EndPointLocation = LocationReqDto.MapFrom(reqDto.EndPointAddress),
                MaxEndTime = reqDto.MaxEndTime,
                MinDateTime = reqDto.MinDateTime
            };
        }
    }
}
