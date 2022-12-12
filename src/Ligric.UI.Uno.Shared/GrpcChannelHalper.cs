//using Grpc.Core;
//using Grpc.Net.Client;
//using Grpc.Net.Client.Web;
using System;
using System.Net.Http;

namespace LigricUno
{
    public static class GrpcChannelHalper
    {
//        public static GrpcChannel GetGrpcChannel()
//        {
//            string address = GetServerAddress();

//            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

//            var httpClientHandler = new HttpClientHandler();

//            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
//            {
//                if (cert.Issuer.Equals("CN=localhost"))
//                    return true;
//                return errors == System.Net.Security.SslPolicyErrors.None;
//            };

//            return GrpcChannel.ForAddress(address, new GrpcChannelOptions
//            {
//#if WINDOWS_UWP
//                HttpHandler = new GrpcWebHandler(httpClientHandler),

//#else
//                HttpClient = new HttpClient(httpClientHandler),

//#endif
//                Credentials = ChannelCredentials.SecureSsl
//            });
//        }

//        private static string GetServerAddress()
//        {
//            var address = "https://3.72.127.66:5010";

//            //---------------------------------------------------------------
//            // TODO : #USE_LOCAL_MODE
//            //---------------------------------------------------------------
//            if (true)
//            {
//#if WINDOWS_UWP
//                address = "https://localhost:5010";
//#endif
//#if __ANDROID__
//                address = "https://10.0.2.2:5010";
//#endif
//            }
//            //---------------------------------------------------------------

//            return address;
//        }
    }
}
