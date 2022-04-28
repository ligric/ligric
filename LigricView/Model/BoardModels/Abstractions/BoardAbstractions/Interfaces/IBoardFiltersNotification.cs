using System.Collections.Generic;

namespace BoardsCore.Abstractions.BoardAbstractions.Interfaces
{
    public delegate void ActionFiltersHandler(object sender, IDictionary<string, string> newCollection);

    public interface IBoardFiltersNotification
    {
        IDictionary<string, string> Filters { get; }

        /// <summary>Событие извещающее об изменении словаря.</summary>
        event ActionFiltersHandler FiltersChanged;
    }
}
