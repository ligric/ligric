namespace AbstractionBoardRepository.Interfaces
{
    public delegate void ActionFiltersHandler(object sender, IDictionary<string, string> newCollection);
    public interface IAdBoardFiltersNotification
    {
        IDictionary<string, string> Filters { get; }

        /// <summary>Событие извещающее об изменении словаря.</summary>
        event ActionFiltersHandler FiltersChanged;
    }
}
