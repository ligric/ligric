using Newtonsoft.Json;
using System.Collections.Generic;

namespace AbstractionBitZlatoRequests.DtoTypes
{
    public class ResponseDto
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("data")]
        public IList<AdDto> Data { get; set; }
    }
}
