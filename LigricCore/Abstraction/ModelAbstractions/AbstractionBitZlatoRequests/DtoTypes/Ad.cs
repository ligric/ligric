using Newtonsoft.Json;

namespace AbstractionBitZlatoRequests.DtoTypes
{
    public class Ad
    {
        [JsonProperty("id")]
        public long Id { get; set; }
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
        public decimal? ownerBalance { get; set; }
    }


    [JsonObject(Title = "limitCurrency")]
    public class LimitCurrency
    {
        [JsonProperty("min")]
        public decimal Min { get; set; }
        [JsonProperty("max")]
        public decimal Max { get; set; }
        [JsonProperty("realMax")]
        public decimal? RealMax { get; set; }
    }

    [JsonObject(Title = "limitCryptocurrency")]
    public class LimitCryptocurrency
    {
        [JsonProperty("min")]
        public decimal Min { get; set; }
        [JsonProperty("max")]
        public decimal Max { get; set; }
        [JsonProperty("realMax")]
        public decimal? RealMax { get; set; }
    }

    [JsonObject(Title = "paymethod")]
    public class Paymethod
    {
        [JsonProperty("id")]
        public ushort Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

}



/*
"id":203942,
"type":"selling",
"cryptocurrency":"BTC",
"currency":"GBP",
"rate":"39191",

"limitCurrency":{
    "min":"100",
    "max":"236",
    "realMax":null
},

"limitCryptocurrency":{
    "min":"0.0025",
    "max":"0.006",
    "realMax":null
},

"paymethod":{
    "id":1850,
    "name":"PaySend"
},
"paymethodId":1850,
"owner":"Sancho",
"ownerLastActivity":1629966150462,
"isOwnerVerificated":true,
"safeMode":true,
"ownerTrusted":false,
"ownerBalance":null
 */