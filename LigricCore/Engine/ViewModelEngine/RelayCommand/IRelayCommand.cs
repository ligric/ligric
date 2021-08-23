using System.Windows.Input;

namespace ViewModelEngine.RelayCommand
{
    /// <summary>Интерфейс добавляющий в интерфейс <see cref="ICommand"/>
    /// метод <see cref="RaiseCanExecuteChanged"/>, поднимающий событие <see cref="ICommand.CanExecuteChanged"/>.</summary>
    public interface IRelayCommand : ICommand
    {
        event EventHandler CanExecuteChanged;

        /// <summary>Подымает (создаёт) событие <see cref="ICommand.CanExecuteChanged"/>.</summary>
        void RaiseCanExecuteChanged();
    }
}
