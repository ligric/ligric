using Ligric.Domain.Client.Base;
using Ligric.Domain.Types.Api;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Utils;

namespace Ligric.Business
{
    public class ApiesService : IApiesService
    {
        private readonly List<ApiClientDto> _availableApies = new List<ApiClientDto>();

        public ApiesService()
        {

        }

        public IReadOnlyCollection<ApiClientDto> AvailableApies => new ReadOnlyCollection<ApiClientDto>(_availableApies);

        public event NotifyCollectionChangedEventHandler? ApiesChanged;

        public Task SaveApiAsync(ApiDto api)
        {
            throw new System.NotImplementedException();
        }

        public Task SetStateAsync(long id, StateEnum state)
        {
            throw new System.NotImplementedException();
        }

        public Task SetStateAsync(IReadOnlyDictionary<long, ApiActivityStateFilter> multiChangesInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}
