using AbstractionBitZlatoRequests.DtoTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbstractionBitZlatoRequests
{
    public interface IBitZlatoRequests
    {
        /// <summary> Получает предложения по указанному фильтру</summary>
        Task<ResponseDto> GetAdBoards(IDictionary<string, string> filters);
    }
}
