using Autofac;
using Ligric.Backend.Application.Users.DomainServices;
using Ligric.Backend.Domain.Entities.Users;

namespace Ligric.Backend.Infrastructure.Domain
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