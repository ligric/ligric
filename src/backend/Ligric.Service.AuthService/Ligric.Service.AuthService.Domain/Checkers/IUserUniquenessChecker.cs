namespace Ligric.Service.AuthService.Domain.Checkers
{
	public interface IUserUniquenessChecker
	{
		bool IsLoginUnique(string userLogin);
	}
}
