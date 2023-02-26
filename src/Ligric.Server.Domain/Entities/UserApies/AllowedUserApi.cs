namespace Ligric.Server.Domain.Entities.UserApies
{
	public class AllowedUserApi
	{
		public virtual long UserApiId { get; set; }
		public virtual string? Name { get; set; }
		public virtual int Permissions { get; set; }
	}
}
