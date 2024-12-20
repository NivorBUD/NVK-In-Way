using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Domain.Models.Profiles
{
    public class DriverProfile : UserProfile
    {
        public List<Car> Cars { get; set; }
    }
}
