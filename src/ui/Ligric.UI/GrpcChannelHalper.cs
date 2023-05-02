using System.Net.Http;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace Ligric.UI
{
	internal static class GrpcChannelHalper
	{
		public static GrpcChannel GetGrpcChannelSsl(string address)
		{
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

			var httpClientHandler = new HttpClientHandler();

#if !__WASM__
			httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
			{
				if (cert != null && cert.Issuer.Equals("CN=localhost"))
					return true;
				return errors == System.Net.Security.SslPolicyErrors.None;
			};
#endif
			return GrpcChannel.ForAddress(address, new GrpcChannelOptions
			{
#if ANDROID
				HttpHandler = new GrpcWebHandler(httpClientHandler),
#elif __WASM__
				HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWebText, httpClientHandler)),
#else
				HttpClient = new HttpClient(httpClientHandler),
#endif

				Credentials = ChannelCredentials.SecureSsl
			});
		}

		public static GrpcChannel GetGrpcChannel(string address)
		{
			return GrpcChannel.ForAddress(address, new GrpcChannelOptions()
			{
				HttpHandler = new GrpcWebHandler(new HttpClientHandler())
			});
		}
	}
}
