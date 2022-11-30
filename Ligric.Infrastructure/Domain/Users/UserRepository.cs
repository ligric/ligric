using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ligric.Domain.Users;
using Ligric.Infrastructure.Database;

namespace Ligric.Infrastructure.Domain.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly DevPaceContext _context;

        public UserRepository(DevPaceContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> GetByIdAsync(UserId id)
        {
            return await this._context.Users
                .SingleAsync(x => x.Id == id);
        }

        public Task SignInAsync(User user)
        {
            //await this._context.Users.AddAsync(user);
            throw new NotImplementedException();
        }

        public Task SignUpAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}