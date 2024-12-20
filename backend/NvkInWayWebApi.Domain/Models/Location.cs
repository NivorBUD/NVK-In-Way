using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvkInWayWebApi.Domain.Models
{
    public class Location
    {
        public Guid Id { get; set; }

        public string TextDescription { get; set; }

        public Coordinate Coordinate { get; set; }

        public Location(string textDescription, Coordinate coordinate)
        {
            TextDescription = textDescription;
            Coordinate = coordinate;
        }
    }
}
