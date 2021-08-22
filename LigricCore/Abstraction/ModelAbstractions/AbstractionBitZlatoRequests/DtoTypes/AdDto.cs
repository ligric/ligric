using Newtonsoft.Json;

namespace AbstractionBitZlatoRequests.DtoTypes
{
    public class AdDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("cryptocurrency")]
        public string Cryptocurrency { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("limitCurrency")]
        public LimitCurrency LimitCurrency { get; set; }

        [JsonProperty("limitCryptocurrency")]
        public LimitCryptocurrency LimitCryptocurrency { get; set; }

        [JsonProperty("paymethod")]
        public Paymethod Paymethod { get; set; }


        [JsonProperty("paymethodId")]
        public short PaymethodId { get; set; }
        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("ownerLastActivity")]
        public long OwnerLastActivity { get; set; }
        [JsonProperty("isOwnerVerificated")]
        public bool IsOwnerVerificated { get; set; }
        [JsonProperty("safeMode")]
        public bool SafeMode { get; set; }
        [JsonProperty("ownerTrusted")]
        public bool OwnerTrusted { get; set; }
        [JsonProperty("ownerBalance")]
        public string ownerBalance { get; set; }
    }


    [JsonObject(Title = "limitCurrency")]
    public class LimitCurrency
    {
        [JsonProperty("min")]
        public double Min { get; set; }
        [JsonProperty("max")]
        public double Max { get; set; }
        [JsonProperty("realMax")]
        public string RealMax { get; set; }
    }

    [JsonObject(Title = "limitCryptocurrency")]
    public class LimitCryptocurrency
    {
        [JsonProperty("min")]
        public double Min { get; set; }
        [JsonProperty("max")]
        public double Max { get; set; }
        [JsonProperty("realMax")]
        public string RealMax { get; set; }
    }

    [JsonObject(Title = "paymethod")]
    public class Paymethod
    {
        [JsonProperty("id")]
        public short Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

}
