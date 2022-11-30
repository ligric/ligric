using Autofac;
using Ligric.Application.Cusomers.DomainServices;
using Ligric.Application.Users.DomainServices;
using Ligric.Domain.Customers;
using Ligric.Domain.Users;

namespace Ligric.Infrastructure.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserUniquenessChecker>()
                .As<IUserUniquenessChecker>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CustomerUniquenessChecker>()
                .As<ICustomerUniquenessChecker>()
                .InstancePerLifetimeScope();
        }
    }
}