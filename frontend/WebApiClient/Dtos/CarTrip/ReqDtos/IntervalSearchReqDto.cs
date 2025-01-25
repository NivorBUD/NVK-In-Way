using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGBotNVK.WebApiClient.Dtos.General.ReqDtos;

namespace TGBotNVK.WebApiClient.Dtos.CarTrip.ReqDtos
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.2.0.0 (NJsonSchema v11.1.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public class IntervalSearchReqDto
    {
        [Newtonsoft.Json.JsonProperty("startPointAddress", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LocationReqDto StartPointAddress { get; set; }

        [Newtonsoft.Json.JsonProperty("endPointAddress", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LocationReqDto EndPointAddress { get; set; }

        [Newtonsoft.Json.JsonProperty("maxEndTime", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? MaxEndTime { get; set; }

        [Newtonsoft.Json.JsonProperty("minDateTime", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? MinDateTime { get; set; }

    }
}
