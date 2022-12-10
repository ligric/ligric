using Ligric.Common.Types;
using Ligric.Common.Types.User;
using System.Reactive.Linq;

namespace Ligric.GrpcServer.Services.LocalTemporary
{
    public class UsersOnlineService
    {
        private readonly Dictionary<long, UserDto> _onlineUsers;

        public UsersOnlineService()
        {
            _onlineUsers = new Dictionary<long, UserDto>();
        }

        // TODO : Need refactoring. Should be insode service
        private event Action<(EventAction, UserDto)> UsersOnlineChanged;

        public  void AddUserOnline(UserDto user)
        {
            if (_onlineUsers.TryAdd((long)user.Id, user))
            {
                UsersOnlineChanged?.Invoke((EventAction.Added, user));
            }
        }

        public void RemoveUserOnline(UserDto user)
        {
            if (_onlineUsers.Remove((long)user.Id))
            {
                UsersOnlineChanged?.Invoke((EventAction.Removed, user));
            }
        }

        // TODO : Need refactoring. Shoul be inside servce
        public IObservable<(EventAction Action, UserDto User)> GetUserOnlinesAsObservable()
        {
            var oldUsers = _onlineUsers.Values.AsEnumerable().Select(x => (EventAction.Added, x)).ToObservable();
            var usersChanged = Observable.FromEvent<(EventAction, UserDto)>((x) => UsersOnlineChanged += x, (x) => UsersOnlineChanged -= x);

            return oldUsers.Concat(usersChanged);
        }
    }
}
