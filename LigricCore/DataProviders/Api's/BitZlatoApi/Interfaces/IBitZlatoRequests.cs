using BitZlatoApi.DtoTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitZlatoApi.Interfaces
{
    public interface IBitZlatoRequests
    {
        /// <summary>Получает список предложений по указанному фильтру</summary>
        Task<Response<Ad[]>> GetAds(IDictionary<string, string> filters = null);
    }
}
