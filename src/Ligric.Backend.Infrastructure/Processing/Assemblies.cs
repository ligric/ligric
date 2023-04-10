using System.Reflection;
using Ligric.Backend.Application.Users.RegisterUser;

namespace Ligric.Backend.Infrastructure.Processing
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(RegisterUserCommand).Assembly;
    }
}