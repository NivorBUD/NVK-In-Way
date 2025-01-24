using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGBotNVK.WebApiClient.Dtos.General.ReqDtos;

namespace TGBotNVK.WebApiClient.Dtos.Driver.ReqDtos
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.2.0.0 (NJsonSchema v11.1.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial record DriverProfileReqDto
    {
        [Newtonsoft.Json.JsonProperty("cars", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<CarReqDto> Cars { get; set; }

        [Newtonsoft.Json.JsonProperty("tgProfileId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long TgProfileId { get; set; }

    }
}
