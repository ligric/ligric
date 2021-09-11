using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace AbstractionRequestSender
{
    public abstract class AbstractRequestSenderService : IRequestSender
    {
        protected readonly IDictionary<string, string> headers;

        protected AbstractRequestSenderService(IDictionary<string, string> headers)
        {
            this.headers = headers;
        }
        protected AbstractRequestSenderService()
        {
            headers = new Dictionary<string, string>();
        }

        public async virtual Task<TResponse> SendHttpRequest<TResponse, TRequest>(string url, HttpMethod method, TRequest request) where TRequest : class
                                                                                                                           where TResponse : class
        {
            TResponse response = null;

            using (var client = new HttpClient())
            {
                var httpRequest = new HttpRequestMessage(method, url);

                foreach (var header in headers)
                {
                    httpRequest.Headers.Add(header.Key, header.Value);
                }

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

        public async virtual Task<TResponse> SendWebRequest<TResponse, TRequest>(string url, HttpMethod method, TRequest request) where TRequest : class
                                                                                                                  where TResponse : class
        {
            var webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = method.Method;

            foreach (var header in headers)
            {
                webRequest.Headers.Add(header.Key, header.Value);
            }

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
