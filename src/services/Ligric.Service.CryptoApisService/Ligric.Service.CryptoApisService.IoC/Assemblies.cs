using System.Reflection;
using Ligric.Service.CryptoApisService.UseCase.Handlers.CreateUserApi;

namespace Ligric.Service.CryptoApisService.IoC
{
	internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(CreateUserApiCommand).Assembly;
    }
}
