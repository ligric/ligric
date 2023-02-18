﻿using Serilog.Formatting.Compact;
using Serilog;
using ILogger = Serilog.ILogger;
using Ligric.Application.Configuration;
using Ligric.Server.Grpc.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ligric.Server.Grpc.Services;
using Ligric.Infrastructure;
using Ligric.Infrastructure.Jwt;
using Ligric.Protos;

namespace Ligric.Server.Grpc
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		private const string LigricConnectionString = "LigricConnectionString";

		private static ILogger? _logger;

		public Startup(IWebHostEnvironment env)
		{
			_logger = ConfigureLogger();
			_logger.Information("Logger configured");

			this._configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json")
				.AddJsonFile($"hosting.{env.EnvironmentName}.json")
				.AddUserSecrets<Startup>()
				.Build();
		}

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			if (services == null)
			{
				throw new NotImplementedException();
			}

			SetJWTAuthorization(services);

			services.AddAuthorization();

			services.AddGrpc();

			services.AddControllers();

			services.AddMemoryCache();

			services.AddHttpContextAccessor();
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
			ServiceProvider serviceProvider = services.BuildServiceProvider();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

			IExecutionContextAccessor executionContextAccessor = new ExecutionContextAccessor(
				serviceProvider.GetService<IHttpContextAccessor>() ?? throw new NotImplementedException());

			var children = this._configuration.GetSection("Caching").GetChildren();
#pragma warning disable CS8604 // Possible null reference argument.
			var cachingConfiguration = children.ToDictionary(child => child.Key, child => TimeSpan.Parse(child.Value));
			var memoryCache = serviceProvider.GetService<IMemoryCache>();
			return ApplicationStartup.Initialize(
				services,
				this._configuration[LigricConnectionString],
				ConfigureLogger(),
				executionContextAccessor);
#pragma warning restore CS8604 // Possible null reference argument.
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseMiddleware<CorrelationMiddleware>();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseGrpcWeb();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();

				endpoints.MapGrpcService<AuthorizationService>().EnableGrpcWeb();
				endpoints.MapGrpcService<UserApisService>().EnableGrpcWeb();

				endpoints.MapGet("/", async context =>
				{
					await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
				});
			});
		}

		private void SetJWTAuthorization(IServiceCollection services)
		{
			var jwtTokenConfig = _configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
			services.AddSingleton(jwtTokenConfig);

			services
				.AddAuthentication(x =>
				{
					x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
				{
					options.RequireHttpsMetadata = true;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = jwtTokenConfig.Issuer,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(jwtTokenConfig.Secret ?? throw new NullReferenceException("Secret is null"))),
						ValidAudience = jwtTokenConfig.Audience,
						ValidateAudience = true,
						ValidateLifetime = true,
						ClockSkew = TimeSpan.FromMinutes(1)
					};
				});

			services.AddHostedService<JwtRefreshTokenCache>();
		}

		private static ILogger ConfigureLogger()
		{
			return new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}")
				.WriteTo.RollingFile(new CompactJsonFormatter(), "logs/logs")
				.CreateLogger();
		}
	}

}
