﻿using Autofac;
using Ligric.Backend.Infrastructure.Domain.Users;
using Ligric.Backend.Domain.Entities.Users;

namespace Ligric.Backend.Infrastructure.Database
{
    public class DataAccessModule : Module
    {
        private readonly string _databaseConnectionString;

        public DataAccessModule(string databaseConnectionString)
        {
            this._databaseConnectionString = databaseConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<SqlConnectionFactory>()
            //    .As<ISqlConnectionFactory>()
            //    .WithParameter("connectionString", _databaseConnectionString)
            //    .InstancePerLifetimeScope();       
            
            //builder.RegisterType<UnitOfWork>()
            //    .As<IUnitOfWork>()
            //    .InstancePerLifetimeScope();

            //builder.RegisterType<UserRepository>()
            //    .As<IUserRepository>()
            //    .InstancePerLifetimeScope();


            //builder
            //    .Register(c =>
            //    {
            //        var dbContextOptionsBuilder = new DbContextOptionsBuilder<DevPaceContext>();
            //        dbContextOptionsBuilder.UseSqlServer(_databaseConnectionString);
            //        dbContextOptionsBuilder
            //            .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

            //        return new DevPaceContext(dbContextOptionsBuilder.Options);
            //    })
            //    .AsSelf()
            //    .As<DbContext>()
            //    .InstancePerLifetimeScope();
        }
    }
}