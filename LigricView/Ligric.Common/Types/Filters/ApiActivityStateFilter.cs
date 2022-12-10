using Common.Enums;
using System.Collections.Generic;

namespace Ligric.Common.Types.Filters
{
    public class ApiActivityStateFilter
    {
        /// <summary>
        /// If this property is null filter will be applied for every one user.
        /// </summary>
        IReadOnlyDictionary<long, StateEnum>? UserActivityStates { get; set; }
    }
}
