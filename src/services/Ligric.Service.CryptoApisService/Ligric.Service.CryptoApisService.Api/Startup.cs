using Serilog.Formatting.Compact;
using Serilog;
using ILogger = Serilog.ILogger;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ligric.Service.CryptoApisService.Infrastructure.Persistence;
using Ligric.Service.CryptoApisService.Infrastructure.MessageBus;
using Ligric.Service.CryptoApisService.Application;
using Ligric.Service.CryptoApisService.Infrastructure;
using Ligric.Service.CryptoApisService.Infrastructure.Jwt;
using Ligric.Service.CryptoApisService.IoC;

namespace Ligric.Service.CryptoApisService.Api
{
	public class Startup
	{
		private readonly IConfiguration _configuration;
		private const string CORS_POLICY = "_corsPolicy";

		private const string ConnectionString = "ConnectionString";

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

			services.AddGrpc();
			services.AddGrpcHttpApi();
			services.AddGrpcReflection();
			services.AddCors(o => o.AddPolicy(CORS_POLICY, builder =>
			{
				builder.AllowAnyOrigin()
					   .AllowAnyMethod()
					   .AllowAnyHeader()
					   .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
			}));

			SetJWTAuthorization(services);
			services.AddAuthorization();


			services.AddMessageBusRegistration(_configuration);
			services.AddPersistenceRegistration(_configuration);

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
				this._configuration[ConnectionString],
				ConfigureLogger(),
				executionContextAccessor);
#pragma warning restore CS8604 // Possible null reference argument.
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

			//app.UseMiddleware<CorrelationMiddleware>();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			//app.UseHttpsRedirection();
			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseGrpcWeb();
			app.UseCors(CORS_POLICY);
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGrpcService<Services.UserApisService>().RequireCors(CORS_POLICY).EnableGrpcWeb();

				endpoints.MapGrpcService<Services.BinanceFuturesOrdersService>().RequireCors(CORS_POLICY).EnableGrpcWeb();
				endpoints.MapGrpcService<Services.BinanceFuturesTradesService>().RequireCors(CORS_POLICY).EnableGrpcWeb();
				endpoints.MapGrpcService<Services.BinanceFuturesPositionsService>().RequireCors(CORS_POLICY).EnableGrpcWeb();
				endpoints.MapGrpcService<Services.BinanceFuturesLeveragesService>().RequireCors(CORS_POLICY).EnableGrpcWeb();

				endpoints.MapGet("/", async context =>
				{
					await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
				});
			});
		}

		private void SetJWTAuthorization(IServiceCollection services)
		{
			var jwtTokenConfig = _configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
			services.AddSingleton(jwtTokenConfig ?? throw new ArgumentException("jwtTokenConfig not initialized at SetJWTAuthorization method"));
			services
				.AddAuthentication(x =>
				{
					x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
				{
					//options.RequireHttpsMetadata = true;
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
