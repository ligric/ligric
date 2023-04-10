namespace Ligric.Service.AuthService.Domain.Entities
{
    public class UserEntity : EntityBase
    {
        public virtual string? UserName { get; set; }

        public virtual string? Salt { get; set; }

        public virtual string? Password { get; set; }
    }
}
