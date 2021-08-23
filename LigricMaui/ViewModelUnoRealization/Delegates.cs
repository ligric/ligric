namespace ViewModelUnoRealization
{
    #region Делегаты для методов команд без параметров.
    public delegate void ExecuteHandler();
    public delegate bool CanExecuteHandler();
    #endregion

    #region Делегаты для методов команд с параметром.
    public delegate void ExecuteHandler<T>(T parameter);
    public delegate bool CanExecuteHandler<T>(T parameter);
    #endregion
}
