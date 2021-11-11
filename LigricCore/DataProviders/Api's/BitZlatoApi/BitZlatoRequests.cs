using AbstractionRequestSender;
using BitZlatoApi.Interfaces;
using BitZlatoApi.Types;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace BitZlatoApi
{
    /// <summary>
    /// API Documentation https://bitzlato.com/en/docsapi
    /// </summary>
    public class BitZlatoRequests : IBitZlatoRequests
    {
        private readonly IRequestSender requestSender;
        private const string _url = "https://bitzlato.com/api/p2p";

        public BitZlatoRequests(string apiKey, string email)
        {
            requestSender = new BitZlatoRequestSenderService(apiKey, email);
        }

        public async Task<ResponseJson<AdJson[]>> GetJsonAdsAsync(IDictionary<string, string> filters = null)
        {
            string url = string.Empty;

            if (filters == null)
                url = $"{_url}/public/exchange/dsa/";
            else
                url = $"{_url}/public/exchange/dsa/?{string.Join("&", filters.Select(kvp => $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}"))}";

            var response = await requestSender.SendHttpRequestAsync<ResponseJson<AdJson[]>, object>(url, HttpMethod.Get, null);
            return response;
        }
    }
}
