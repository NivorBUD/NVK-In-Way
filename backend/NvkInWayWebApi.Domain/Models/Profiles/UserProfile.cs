using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Domain.Models.Profiles
{
    public abstract class UserProfile
    {
        public long TgProfileId { get; set; }

        public float? Rating { get; set; }

        public int TripsCount { get; set; }
    }
}
