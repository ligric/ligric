using System.Net.Http;
using System.Threading.Tasks;

namespace AbstractionRequestSender
{
    public interface IRequestSender
    {
        Task<TResponse> SendHttpRequestAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest request)
            where TResponse : class where TRequest : class;
        Task<TResponse> SendWebRequestAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest request) 
            where TResponse : class where TRequest : class;
    }
}
