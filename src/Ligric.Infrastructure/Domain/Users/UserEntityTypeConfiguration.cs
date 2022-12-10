using Ligric.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ligric.Infrastructure.Domain.Users
{
    //internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    //{
    //    public void Configure(EntityTypeBuilder<User> builder)
    //    {
    //        builder.ToTable("Users", SchemaNames.Ligric);
            
    //        builder.HasKey(b => b.Id);

    //        builder.Property("Login").HasColumnName("Login");
    //        builder.Property("Password").HasColumnName("PassHash");
    //        builder.Property("Email").HasColumnName("Email");
    //    }
    //}
}