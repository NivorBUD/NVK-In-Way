using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Domain.Models;

namespace NvkInWayWebApi.Application.Common.Dtos.CarTrip.ResDtos
{
    public class NotifyTripResDto
    {
        public List<long> Profiles { get; set; }

        public NotifyTripInfo NotifyInfo { get; set; }

        public static NotifyTripResDto MapFrom(KeyValuePair<Trip, List<long>> notifyEntry)
        {
            return new NotifyTripResDto
            {
                Profiles = notifyEntry.Value,
                NotifyInfo = NotifyTripInfo.MapFrom(notifyEntry.Key)
            };
        }
    }
}
