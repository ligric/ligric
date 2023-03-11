using Ligric.Application.Configuration.Queries;

namespace TemporaryProjectJustForCopyPast.Application.Users.CheckUserExists
{
	public class UserNameIsUniqueQuery : IQuery<bool>
	{
		public string UserName { get; }

		public UserNameIsUniqueQuery(string username)
		{
			UserName = username;
		}
	}
}
