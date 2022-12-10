using BitZlatoApi.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitZlatoApi.Interfaces
{
    public interface IBitZlatoRequests
    {
        /// <summary>Получает список предложений по указанному фильтру</summary>
        Task<ResponseJson<AdJson[]>> GetJsonAdsAsync(IDictionary<string, string> filters = null);
    }
}
