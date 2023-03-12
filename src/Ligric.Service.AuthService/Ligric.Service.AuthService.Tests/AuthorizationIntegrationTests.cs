using Ligric.Protos;
using Google.Protobuf.WellKnownTypes;
using FluentAssertions;
using System.Security.Cryptography.X509Certificates;
using Grpc.Core;
using FlueFlame.AspNetCore.Grpc.Modules.Unary;
using System.Reflection.PortableExecutable;
using Faker;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Ligric.Tests
{
	public sealed class AuthorizationIntegrationTests : TestBase
	{
		private StringValue RandomGuidRequest => new() { Value = Guid.NewGuid().ToString() };

        [Fact]
        public async Task GetJwtToken_FromAuthorization_ReturnsIsSuccessAndValidToken()
        {
			// Arrange
			var request = new SignInRequest() { Login = "test", Password = "test" };
			var client = GrpcHost.CreateClient<Authorization.AuthorizationClient>();

			// Act
			var act = client.Unary.Call(c => c.SignIn(request));

			// Assert
			act.Response.AssertThat(e => e.Result.IsSuccess.Should().BeTrue() );
			act.Response.AssertThat(e => e.JwtToken.AccessToken.Should().NotBeNullOrEmpty() );
			act.Response.AssertThat(e => e.JwtToken.RefreshToken.Should().NotBeNullOrEmpty() );
        }

		[Fact]
		public async Task CheckJwtToken_OneMinuteExpirationTime_ReturnsTrue()
		{
			// Arrange
			var request = new SignInRequest() { Login = "test", Password = "test" };
			var client = GrpcHost.CreateClient<Authorization.AuthorizationClient>();

			var actLogin = client.Unary.Call(c => c.SignIn(request));

			string accessToken = null;

			actLogin.Response.AssertThat(e =>
			{
				accessToken = e.JwtToken.AccessToken;
			});

			var headers = new Metadata
			{
				{ "Authorization", $"Bearer {accessToken}" }
			};

			var actExpirationAt = client.Unary.Call(c => c.GetTokenExpirationTime(new Empty(), headers));

			actExpirationAt.Response.AssertThat(e =>
			{
				//var afsaf = e.ExpirationAt.ToDateTime();
			});



			// Assert


			// Assert
			//act.Response.AssertThat(async e =>
			//{
			//	var tokenResponse = e.JwtToken;
			//	await Task.Delay(1000);
			//});
		}

		[Fact]
		public async Task CheckJwtToken_ExpirationTimeFromUtcNow_ReturnsTrue()
		{
			//// Arrange
			//var request = new SignInRequest() { Login = "test", Password = "test" };
			//var client = GrpcHost.CreateClient<Authorization.AuthorizationClient>();

			//// Act
			//var act = client.Unary.Call(c => c.SignIn(request));

			//// Assert
			//act.Response.AssertThat(async e =>
			//{
			//	var expiration = e.JwtToken.ExpirationAt.ToDateTime();
			//	var now = DateTime.UtcNow;
			//	var difference = now - expiration;
			//});
		}

		//[Trait("Category", "Integration")]
		//[Fact]
		//public async Task Given_RandomName_When_CallingSayHelloServerStreamEndpoint_Return_GreetingInChunks()
		//{
		//    // Arrange
		//    var greeterClient = new Greeter.GreeterClient(_channel);
		//    var userName = _fixture.Create<string>();
		//    // Act
		//    var responseMessage = greeterClient.SayHelloServerStream(new HelloRequest() { Name = userName });
		//    // Assert
		//    var expectedResult = $"Hello {userName}";
		//    var i = 0;
		//    while (await responseMessage.ResponseStream.MoveNext())
		//    {
		//        var value = responseMessage.ResponseStream.Current;
		//        Assert.Equal(expectedResult[i++].ToString(), value.Message);
		//    }
		//}

		//[Trait("Category", "Integration")]
		//[Fact]
		//public async Task Given_RandomNameInChunks_When_CallingSayHelloClientStreamEndpoint_Return_GreetingMessage()
		//{
		//    // Arrange
		//    var greeterClient = new Greeter.GreeterClient(_channel);
		//    var userName = _fixture.Create<string>();
		//    // Act
		//    var clientStreamingCall = greeterClient.SayHelloClientStream();
		//    foreach (var nameChunk in userName)
		//    {
		//        await clientStreamingCall.RequestStream.WriteAsync(new HelloRequest() { Name = nameChunk.ToString() });
		//    }
		//    await clientStreamingCall.RequestStream.CompleteAsync();
		//    // Assert
		//    var responseMessage = await clientStreamingCall.ResponseAsync;
		//    Assert.Equal($"Hello {userName}", responseMessage.Message);
		//}

		//[Trait("Category", "Integration")]
		//[Fact]
		//public async Task Given_RandomNameInChunks_When_CallingSayHelloBiStreamEndpoint_Return_GreetingMessageInChunks()
		//{
		//    // Arrange
		//    var greeterClient = new Greeter.GreeterClient(_channel);
		//    var userName = _fixture.Create<string>();
		//    // Act
		//    var call = greeterClient.SayHelloBiStream();
		//    foreach (var nameChunk in userName)
		//    {
		//        await call.RequestStream.WriteAsync(new HelloRequest() { Name = nameChunk.ToString() });
		//    }
		//    await call.RequestStream.CompleteAsync();
		//    // Assert
		//    var expectedResult = userName;
		//    var i = 0;
		//    while (await call.ResponseStream.MoveNext())
		//    {
		//        var value = call.ResponseStream.Current;
		//        Assert.Equal(expectedResult[i++].ToString(), value.Message);
		//    }
		//}
	}
}
