using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FlueFlame.AspNetCore;
using FlueFlame.AspNetCore.Grpc;
using FlueFlame.Http.Host;
using Grpc.Core;
using Grpc.Net.Client;
using Ligric.Service.AuthService.Api;
using Ligric.Service.AuthService.Infrastructure.Nhibernate.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Ligric.Service.AuthService.Tests.App_Infrastructure;

public abstract class FlueFlameIntegrationTestBase : IDisposable
{
	protected IFlueFlameHttpHost HttpHost { get; }
	protected IFlueFlameGrpcHost GrpcHost { get; }
	protected IServiceProvider ServiceProvider { get; }
	protected TestServer TestServer { get; }

	protected DataProvider DataProvider => ServiceProvider.CreateScope()
		.ServiceProvider.GetRequiredService<DataProvider>();

	protected FlueFlameIntegrationTestBase()
	{
		var webApp = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder =>
			{
				builder.UseEnvironment("Test");
			});

		TestServer = webApp.Server;
		ServiceProvider = webApp.Services;

		var builder = FlueFlameAspNetBuilder.CreateDefaultBuilder(webApp)
			.ConfigureHttpClient(c =>
			{
				//Configure HttpClient for all FlueFlame hosts such as HttpHost, GrpcHost, SignalRHost...

				//Save JWT token to default request headers
				//c.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetJwtToken()}");
			});

		HttpHost = builder.BuildHttpHost(b =>
		{
			//Use System.Text.Json serializer
			b.UseTextJsonSerializer();

			//Configure HttpClient only for FlueFlameHttpHost
			b.ConfigureHttpClient(client => { });
		});

		GrpcHost = builder.BuildGrpcHost(b =>
		{
			//Configure FlueFlameGrpcHost here

			//Configure HttpClient only for FlueFlameGrpcHost
			b.ConfigureHttpClient(client =>
			{
				//client.DefaultRequestHeaders.Save("Authorization", $"Bearer {GetJwtToken()}");
			});

			//Use custom GrpcChannelOptions
			b.UseCustomGrpcChannelOptions(new GrpcChannelOptions()
			{
				MaxRetryAttempts = 1,
				Credentials = ChannelCredentials.Create(new SslCredentials(),
					CallCredentials.FromInterceptor((context, metadata) =>
					{
						//metadata.Add("Authorization", $"Bearer {GetJwtToken()}");
						return Task.CompletedTask;
					}))
			});
		});
	}

	public void Dispose()
	{
		using var scope = ServiceProvider.CreateScope();
		//ctx.Database.EnsureDeleted();
		GC.SuppressFinalize(this);
	}
}
