namespace Ligric.Domain.Users
{
    public interface IUserUniquenessChecker
    {
        bool IsLoginUnique(string userLogin);
    }
}