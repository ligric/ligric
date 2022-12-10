using Ligric.Common.Types;
using Ligric.Common.Types.Api;
using System.Reactive.Linq;

namespace Ligric.GrpcServer.Services.LocalTemporary
{
    public class UserapiesLocalService
    {
        private readonly Dictionary<long, ApiDto> _apies;

        public UserapiesLocalService()
        {
            _apies = new Dictionary<long, ApiDto>();
        }

        // TODO : Need refactoring. Should be insode service
        private event Action<(EventAction, ApiDto)> ApiesChanged;

        public  void AddApi(ApiDto api)
        {
            if (_apies.TryAdd((long)api.Id, api))
            {
                ApiesChanged?.Invoke((EventAction.Added, api));
            }
        }

        public void RemoveApi(ApiDto api)
        {
            if (_apies.Remove((long)api.Id))
            {
                ApiesChanged?.Invoke((EventAction.Removed, api));
            }
        }

        // TODO : Need refactoring. Shoul be inside servce
        public IObservable<(EventAction Action, ApiDto Api)> GetUserOnlinesAsObservable()
        {
            var oldApies = _apies.Values.AsEnumerable().Select(x => (EventAction.Added, x)).ToObservable();
            var newApies = Observable.FromEvent<(EventAction, ApiDto)>((x) => ApiesChanged += x, (x) => ApiesChanged -= x);

            return oldApies.Concat(newApies);
        }
    }
}
