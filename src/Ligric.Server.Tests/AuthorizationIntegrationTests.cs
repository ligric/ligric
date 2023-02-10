using Ligric.Protos;
using Google.Protobuf.WellKnownTypes;
using FluentAssertions;

namespace Ligric.Server.Tests
{
	public sealed class AuthorizationIntegrationTests : TestBase
	{
		private StringValue RandomGuidRequest => new() { Value = Guid.NewGuid().ToString() };

        [Fact]
        public async Task UserAuthorization_ReturnsIsSuccess()
        {
			// Arrange
			var request = new SignInRequest() { Login = "test", Password = "test" };
			var client = GrpcHost.CreateClient<Authorization.AuthorizationClient>();

			// Act
			var act = client.Unary.Call(c => c.SignIn(request));

			// Assert
			act.Response.AssertThat(e => e.Result.IsSuccess.Should().BeTrue());

			// Arrange
			//var authorizationClient = new Authorization.AuthorizationClient(_channel);
            //Assert.True(response.Result.IsSuccess);
            //Assert.NotNull(response.JwtToken?.Token);
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
