using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FlueFlame.AspNetCore;
using FlueFlame.AspNetCore.Grpc;
using FlueFlame.Http.Host;
using Grpc.Core;
using Grpc.Net.Client;
using Ligric.Server.Grpc;
using Ligric.Server.Tests.TestDataBuilders;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Ligric.Server.Tests;

public abstract class TestBase : IDisposable
{
	protected IFlueFlameHttpHost HttpHost { get; }
	protected IFlueFlameGrpcHost GrpcHost { get; }
	protected IServiceProvider ServiceProvider { get; }
	protected TestServer TestServer { get; }
	//protected EmployeeContext EmployeeContext => ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<EmployeeContext>();

	protected TestBase()
	{
		var webApp = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder =>
			{
				// Set data base
				builder.ConfigureServices(services =>
				{
					//services.Remove<IConnectionSettingsProvider>();

					//Configure your services here
					//var dbContextDescriptor = services.SingleOrDefault(
					//	d => d.ServiceType ==
					//		 typeof(IConnectionSettingsProvider));

					//services.Remove(dbContextDescriptor);

					////Unique Database name for each test.
					//var dbName = $"Employee_{Guid.NewGuid()}";

					////Use InMemory Database
					//services.AddDbContext<EmployeeContext>(x => x.UseInMemoryDatabase(dbName));
				});
			});

		TestServer = webApp.Server;
		ServiceProvider = webApp.Services;

		var builder = FlueFlameAspNetBuilder.CreateDefaultBuilder(webApp)
			.ConfigureHttpClient(c =>
			{
				//Configure HttpClient for all FlueFlame hosts such as HttpHost, GrpcHost, SignalRHost...
				
				//Add JWT token to default request headers
				c.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetJwtToken()}");
			});

		HttpHost = builder.BuildHttpHost(b =>
		{
			//Use System.Text.Json serializer
			b.UseTextJsonSerializer();
			
			//Configure HttpClient only for FlueFlameHttpHost
			b.ConfigureHttpClient(client =>
			{
				
			});
		});
		
		GrpcHost = builder.BuildGrpcHost(b =>
		{
			//Configure FlueFlameGrpcHost here
			
			//Configure HttpClient only for FlueFlameGrpcHost
			b.ConfigureHttpClient(client =>
			{
				//client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetJwtToken()}");
			});

			//Use custom GrpcChannelOptions
			b.UseCustomGrpcChannelOptions(new GrpcChannelOptions()
			{
				MaxRetryAttempts = 1,
				Credentials = ChannelCredentials.Create(new SslCredentials(),
					CallCredentials.FromInterceptor((context, metadata) =>
					{
						metadata.Add("Authorization", $"Bearer {GetJwtToken()}");
						return Task.CompletedTask;
					}))
			});
		});
	}

	protected string GetJwtToken(string role = "admin", TimeSpan? lifetime = null)
	{
		var jwt = new JwtSecurityToken(
			issuer: AuthOptions.ISSUER,
			audience: AuthOptions.AUDIENCE,
			notBefore: DateTime.UtcNow,
			claims: new List<Claim>() { new(ClaimsIdentity.DefaultRoleClaimType, role) },
			expires: DateTime.UtcNow.Add(lifetime ?? TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
			signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
				SecurityAlgorithms.HmacSha256));

		return new JwtSecurityTokenHandler().WriteToken(jwt);
	}

	public void Dispose()
	{
		using var scope = ServiceProvider.CreateScope();
		// Set data context

		//var ctx = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
		//ctx.Database.EnsureDeleted();
		GC.SuppressFinalize(this);
	}
}
