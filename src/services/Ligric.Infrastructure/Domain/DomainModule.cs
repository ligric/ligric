using Autofac;
using Ligric.Application.Users.DomainServices;
using Ligric.Domain.Entities.Users;

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