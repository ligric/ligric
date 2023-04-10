using System;
using Ligric.Service.AuthService.Application.Repositories;
using Ligric.Service.AuthService.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ligric.Service.AuthService.Infrastructure.Persistence
{
	public static class PersistenceContainer
	{
		public static IServiceCollection AddPersistenceRegistration(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			services.Configure<DataProtectionTokenProviderOptions>(opt =>
				opt.TokenLifespan = TimeSpan.FromHours(2));

			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}
	}
}
