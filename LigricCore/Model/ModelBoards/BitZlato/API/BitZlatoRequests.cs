using AbstractionBitZlatoRequests;
using AbstractionBitZlatoRequests.DtoTypes;
using JsonWebToken;

namespace BoardRepository.BitZlato.API
{
    public class BitZlatoRequests : IBitZlatoRequests
    {
        private string apiKey;

        private string email;

        private static readonly Random rnd = new Random();

        public BitZlatoRequests(string apiKey, string email)
        {
            this.apiKey = apiKey; this.email = email;
        }

        private string GenerateToken()
        {
            var privJwk = Jwk.FromJson(apiKey);

            var descriptor = new JwsDescriptor()
            {
                Algorithm = SignatureAlgorithm.EcdsaSha256,
                SigningKey = privJwk,
                IssuedAt = DateTime.UtcNow,
                Audience = "usr",
                JwtId = rnd.Next().ToString("X"),
                KeyId = 2.ToString()
            };
            descriptor.AddClaim("email", email);

            var writer = new JwtWriter();
            return writer.WriteTokenString(descriptor);
        }


        public Task<ResponseDto> GetAdBoards(IDictionary<string, string> filters)
        {
            ResponseDto result;
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

            return null;
        }
    }
}
