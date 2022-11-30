using Microsoft.EntityFrameworkCore;
using Ligric.Domain.Users;
using Ligric.Infrastructure.Processing.InternalCommands;
using Ligric.Infrastructure.Processing.Outbox;
using Ligric.Domain.Customers;

namespace Ligric.Infrastructure.Database
{
    public class DevPaceContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public DbSet<InternalCommand> InternalCommands { get; set; }

        public DevPaceContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DevPaceContext).Assembly);
        }
    }
}
