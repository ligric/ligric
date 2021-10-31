using System.Windows.Input;

namespace LigricMvvm.RelayCommand
{
    /// <summary>Реализация RelayCommand для методов с обобщённым параметром.</summary>
    /// <typeparam name="T">Тип параметра методов.</typeparam>
    public class RelayCommand<T> : RelayCommand, ICommand
    {
        /// <inheritdoc cref="RelayCommand(ExecuteHandler, CanExecuteHandler)"/>
        public RelayCommand(ExecuteHandler<T> execute, CanExecuteHandler<T> canExecute = null)
            : base
            (
                  p => execute((T)p),
                  canExecute == null ? null : p => canExecute((T)p)
            )
        { }
    }
}
