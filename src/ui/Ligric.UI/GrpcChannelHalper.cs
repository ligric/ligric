using System.Net.Http;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace Ligric.UI
{
	internal static class GrpcChannelHalper
	{
		public static GrpcChannel GetGrpcChannelSsl()
		{
			var address = GetServerAddress();

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

		public static GrpcChannel GetGrpcChannel()
		{
			return GrpcChannel.ForAddress("http://localhost:8080", new GrpcChannelOptions()
			{
				HttpHandler = new GrpcWebHandler(new HttpClientHandler())
			});

		}


		private static string GetServerAddress()
		{
			var address = "https://3.72.127.66:5010";

			//---------------------------------------------------------------
			// TODO : #USE_LOCAL_MODE
			//---------------------------------------------------------------
			if (true)
			{
#if ANDROID
                address = "https://10.0.2.2:5010";
#else
				address = "https://localhost:5010";
#endif
			}
			//---------------------------------------------------------------

			return address;
		}
	}
}
