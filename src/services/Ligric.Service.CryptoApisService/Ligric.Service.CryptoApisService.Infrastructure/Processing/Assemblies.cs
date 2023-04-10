using System.Reflection;
using Ligric.Service.CryptoApisService.Infrastructure.Handlers.CreateUserApi;

namespace Ligric.Infrastructure.Processing
{
	internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(CreateUserApiCommand).Assembly;
    }
}
