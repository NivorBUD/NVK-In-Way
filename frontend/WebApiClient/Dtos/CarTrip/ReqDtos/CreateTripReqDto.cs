using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGBotNVK.WebApiClient.Dtos.General.ReqDtos;

namespace TGBotNVK.WebApiClient.Dtos.CarTrip.ReqDtos
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.2.0.0 (NJsonSchema v11.1.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial record CreateTripReqDto
    {
        [Newtonsoft.Json.JsonProperty("startPoint", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LocationReqDto StartPoint { get; set; }

        [Newtonsoft.Json.JsonProperty("endPoint", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LocationReqDto EndPoint { get; set; }

        [Newtonsoft.Json.JsonProperty("driveStartTime", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset DriveStartTime { get; set; }

        [Newtonsoft.Json.JsonProperty("driveEndTime", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset DriveEndTime { get; set; }

        [Newtonsoft.Json.JsonProperty("totalPlaces", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int TotalPlaces { get; set; }

        [Newtonsoft.Json.JsonProperty("tripCost", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double TripCost { get; set; }

        [Newtonsoft.Json.JsonProperty("carLocation", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CarLocation { get; set; }

        [Newtonsoft.Json.JsonProperty("tripCar", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public OnlyCarIdsReqDto TripCar { get; set; }

    }
}
