using AbstractionRequestSender;
using JsonWebToken;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace BoardRepository.BitZlato.API
{
    public class BitZlatoRequestSenderService : IRequestSenderService
    {
        private string apiKey;
        private string email;

        private static readonly Random rnd = new Random();

        private readonly Dictionary<string, string> _headers;
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

        public BitZlatoRequestSenderService(Dictionary<string, string> headers, string api, string email)
        {
            this.apiKey = api; this.email = email;
            _headers = headers;
        }
        public BitZlatoRequestSenderService(string api, string email)
        {
            this.apiKey = api; this.email = email;
            _headers = new Dictionary<string, string>();
        }

        public async Task<TResponse> SendHttpRequest<TResponse, TRequest>(string url, HttpMethod method, TRequest request) where TRequest : class
                                                                                                                           where TResponse : class
        {
            TResponse response = null;

            using (var client = new HttpClient())
            {
                var httpRequest = new HttpRequestMessage(method, url);

                foreach (var header in _headers)
                {
                    httpRequest.Headers.Add(header.Key, header.Value);
                }
                httpRequest.Headers.Add("Bearer", GenerateToken());

                if (request != null && method != HttpMethod.Get)
                {
                    string json = JsonConvert.SerializeObject(request);
                    var content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    httpRequest.Content = content;
                }


                var httpResponse = await client.SendAsync(httpRequest);


                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<TResponse>(json);
                }
            }

            return response;
        }

        public async Task<TResponse> SendWebRequest<TResponse, TRequest>(string url, HttpMethod method, TRequest request) where TRequest : class
                                                                                                                  where TResponse : class
        {
            var webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = method.Method;

            foreach (var header in _headers)
            {
                webRequest.Headers.Add(header.Key, header.Value);
            }
            webRequest.Headers.Add("Bearer", GenerateToken());

            if (method != HttpMethod.Get && request != null)
            {
                string json = JsonConvert.SerializeObject(request);
                byte[] bytes = Encoding.UTF8.GetBytes(json);

                webRequest.ContentType = "application/json";
                webRequest.ContentLength = bytes.Length;

                using (var stream = webRequest.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            else
            {
                webRequest.ContentLength = 0;
            }

            TResponse response = null;
            using (var webResponse = await webRequest.GetResponseAsync())
            using (var stream = webResponse.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    string json = await reader.ReadToEndAsync();
                    response = JsonConvert.DeserializeObject<TResponse>(json);
                }
            }

            return response;
        }
    }
}
