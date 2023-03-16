using System.Reflection;
using Ligric.Service.AuthService.UseCase.Handlers.RegisterUser;

namespace Ligric.Service.AuthService.IoC
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(RegisterUserCommand).Assembly;
    }
}
