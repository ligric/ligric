using Serilog.Formatting.Compact;
using Serilog;
using ILogger = Serilog.ILogger;
using Ligric.Application.Configuration;
using Ligric.Server.Grpc.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Ligric.Infrastructure;
using Ligric.Infrastructure.Caching;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ligric.Server.Grpc.Services;
using System.Reflection;
using MediatR;
using Ligric.Server.Grpc.Services.LocalTemporary;

namespace Ligric.Server.Grpc
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private const string LigricConnectionString = "LigricConnectionString";

        private static ILogger _logger;

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
            SetJWTAuthorization(services);
            //services.AddOptions();

            //services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddGrpc();

            services.AddControllers();

            services.AddMemoryCache();

            services.AddSingleton<UsersOnlineService>();
            services.AddSingleton<UserapiesLocalService>();



            //services.AddProblemDetails(x =>
            //{
            //    x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
            //    x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            //});


            services.AddHttpContextAccessor();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            IExecutionContextAccessor executionContextAccessor = new ExecutionContextAccessor(serviceProvider.GetService<IHttpContextAccessor>());

            var children = this._configuration.GetSection("Caching").GetChildren();
            var cachingConfiguration = children.ToDictionary(child => child.Key, child => TimeSpan.Parse(child.Value));
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            return ApplicationStartup.Initialize(
                services,
                this._configuration[LigricConnectionString],
                new MemoryCacheStore(memoryCache, cachingConfiguration),
                ConfigureLogger(),
                executionContextAccessor);
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
                endpoints.MapGrpcService<UserApiesService>().EnableGrpcWeb();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }

        private void SetJWTAuthorization(IServiceCollection builder)
        {
            builder.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = _configuration.GetValue<string>("JwtAudience"),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration.GetValue<string>("JwtIssuer"),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtKey"))),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });

            builder.AddAuthorization();
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
