using Autofac;
using Ligric.Service.AuthService.Domain.Checkers;
using Ligric.Service.AuthService.Infrastructure.Domain;

namespace Ligric.Infrastructure.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserUniquenessChecker>()
                .As<IUserUniquenessChecker>()
                .InstancePerLifetimeScope();

            //builder.RegisterType<CustomerUniquenessChecker>()
            //    .As<ICustomerUniquenessChecker>()
            //    .InstancePerLifetimeScope();
        }
    }
}
