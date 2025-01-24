using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGBotNVK.WebApiClient.Dtos.General.ResDtos;

namespace TGBotNVK.WebApiClient.Dtos.CarTrip.ResDtos
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.2.0.0 (NJsonSchema v11.1.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial record GetActiveTripsResDto
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid Id { get; set; }

        [Newtonsoft.Json.JsonProperty("carLocation", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CarLocation { get; set; }

        [Newtonsoft.Json.JsonProperty("startPoint", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LocationResDto StartPoint { get; set; }

        [Newtonsoft.Json.JsonProperty("endPoint", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LocationResDto EndPoint { get; set; }

        [Newtonsoft.Json.JsonProperty("tripStartTime", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset TripStartTime { get; set; }

        [Newtonsoft.Json.JsonProperty("tripEndTime", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset TripEndTime { get; set; }

        [Newtonsoft.Json.JsonProperty("totalPlaces", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int TotalPlaces { get; set; }

        [Newtonsoft.Json.JsonProperty("driveCost", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double DriveCost { get; set; }

        [Newtonsoft.Json.JsonProperty("bookedPlaces", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int BookedPlaces { get; set; }

        [Newtonsoft.Json.JsonProperty("tripCar", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public CarResDto TripCar { get; set; }

        [Newtonsoft.Json.JsonProperty("driverId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long DriverId { get; set; }

    }
}
