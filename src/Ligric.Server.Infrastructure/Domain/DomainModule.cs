using Autofac;
using Ligric.Server.Application.Users.DomainServices;
using Ligric.Server.Domain.Entities.Users;

namespace Ligric.Server.Infrastructure.Domain
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