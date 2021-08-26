using AbstractionBitZlatoRequests;
using AbstractionBitZlatoRequests.DtoTypes;
using AbstractionRequestSender;
using System.Web;

namespace BoardRepository.BitZlato.API
{
    public class BitZlatoRequests : IBitZlatoRequestsService
    {
        private IRequestSenderService _requestSender;
        private const string _url = "https://bitzlato.com/api/p2p";

        public BitZlatoRequests(string apiKey, string email)
        {
            _requestSender = new BitZlatoRequestSenderService(apiKey, email);
        }


        public async Task<Response<Ad[]>> GetAdsFromFilters(IDictionary<string, string> filters)
        {
            var url = $"{_url}/public/exchange/dsa/?{string.Join("&", filters.Select(kvp => $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}"))}";
            var response = await _requestSender.SendHttpRequest<Response<Ad[]>, object>(url, HttpMethod.Get, null);
            return response;
        }
    }
}
