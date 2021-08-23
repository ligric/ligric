using Newtonsoft.Json;
using System.Collections.Generic;

namespace AbstractionBitZlatoRequests.DtoTypes
{
    public class Response<T>
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
