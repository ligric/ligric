using Ligric.Domain.Customers;
using Ligric.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ligric.Infrastructure.Domain.Customers
{
    internal sealed class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers", SchemaNames.Ligric);

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property("Email").HasColumnName("Email");
            builder.Property("Name").HasColumnName("Name");
            builder.Property("CompanyName").HasColumnName("CompanyName");
            builder.Property("Phone").HasColumnName("Phone");
        }
    }
}