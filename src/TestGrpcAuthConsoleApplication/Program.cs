using Grpc.Net.Client;
using Ligric.Rpc.Contracts;

var channel = GrpcChannel.ForAddress("https://localhost:8085");
var client = new Auth.AuthClient(channel);

var response = await client.SignInAsync(new SignInRequest
{
	 Login = "limeniye",
	 Password = "12345"
});

Console.ReadLine();
