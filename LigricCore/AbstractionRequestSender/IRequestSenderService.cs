namespace AbstractionRequestSender
{
    public interface IRequestSenderService
    {
        Task<TResponse> SendHttpRequest<TResponse, TRequest>(string url, HttpMethod method, TRequest request) where TResponse : class
                                                                                                              where TRequest : class;
        Task<TResponse> SendWebRequest<TResponse, TRequest>(string url, HttpMethod method, TRequest request) where TResponse : class
                                                                                                       where TRequest : class;
    }
}
