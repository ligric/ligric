using System.Threading.Tasks;

namespace Ligric.Domain.Users
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(UserId id);

        Task SignInAsync(User user);

        Task SignUpAsync(User user);
    }
}