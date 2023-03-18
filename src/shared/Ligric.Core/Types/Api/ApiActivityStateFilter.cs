using System.Collections.Generic;
using Utils;

namespace Ligric.Core.Ligric.Core.Types.Api
{
    public class ApiActivityStateFilter
    {
        /// <summary>
        /// If this property is null filter will be applied for every one user.
        /// </summary>
        IReadOnlyDictionary<long, StateEnum>? UserActivityStates { get; set; }
    }
}
