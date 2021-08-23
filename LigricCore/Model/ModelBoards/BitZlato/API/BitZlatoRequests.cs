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
            //string json = JsonConvert.SerializeObject(filters, Formatting.Indented);
            //json = HttpUtility.UrlEncode(json);

            //https://bitzlato.com/api/p2p/public/exchange/dsa/

            var url = $"{_url}/public/exchange/dsa/?{HttpUtility.UrlEncode(string.Join("&", filters.Select(kvp => $"{kvp.Key}={kvp.Value}")))}";
            //string url = $"{_url}/public/exchange/dsa/?params={json}";
            var response = await _requestSender.SendHttpRequest<Response<Ad[]>, object>(url, HttpMethod.Get, null);
            return response;
        }

        //public Task<ResponseDto> GetAdsFromFilters(IDictionary<string, string> filters)
        //{
            //ResponseDto result;
            //using (var request = new HttpRequest())
            //{
            //    request.AddHeader("Bearer", GenerateToken());

            //    var str = JsonConvert.SerializeObject(new
            //    {
            //        limit = 15,
            //        //amount = 1,
            //        //amountType = "currency", // хз
            //        cryptocurrency = filters["cryptocurrency"],
            //        currency = filters["currency"],
            //        isOwnerActive = filters["isOwnerActive"],
            //        isOwnerVerificated = filters["isOwnerVerificated"],
            //        lang = "ru",
            //        skip = 0,
            //        type = filters["type"]
            //    });

            //    var response = request.Get("https://bitzlato.com/api/p2p/public/exchange/dsa/");
            //    string responseJsonResult = response.ToString();

            //    result = JsonConvert.DeserializeObject<ResponseDto>(responseJsonResult);
            //}

           // return null;
        //}
    }
}
