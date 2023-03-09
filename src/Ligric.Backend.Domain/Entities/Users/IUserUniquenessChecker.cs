namespace Ligric.Backend.Domain.Entities.Users
{
    public interface IUserUniquenessChecker
    {
        bool IsLoginUnique(string userLogin);
    }
}