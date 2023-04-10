namespace Ligric.Domain.Entities.Users
{
    public interface IUserUniquenessChecker
    {
        bool IsLoginUnique(string userLogin);
    }
}