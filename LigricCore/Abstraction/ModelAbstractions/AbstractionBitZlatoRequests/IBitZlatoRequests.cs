using AbstractionBitZlatoRequests.DtoTypes;

namespace AbstractionBitZlatoRequests
{
    public interface IBitZlatoRequestsService
    {
        /// <summary>Получает предложения по указанному фильтру</summary>
        Task<Response<Ad[]>> GetAdsFromFilters(IDictionary<string, string> filters);
    }
}
