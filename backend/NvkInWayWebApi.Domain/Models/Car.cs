using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Domain.Models
{
    public class Car
    {
        public Guid Id { get; set; }

        public Guid DriverId { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public string Color { get; set; }
    }
}
