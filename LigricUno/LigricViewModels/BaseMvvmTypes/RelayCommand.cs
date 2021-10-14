using System;
using System.Windows.Input;

namespace LigricViewModels.BaseMvvmTypes
{
    /* 
     * https://www.cyberforum.ru/wpf-silverlight/thread2738784.html#post15042396
     */

    #region Delegates
    public delegate void ExecuteHandler();
    public delegate bool CanExecuteHandler();

    public delegate void ExecuteHandler<T>(T parameter);
    public delegate bool CanExecuteHandler<T>(T parameter);
    #endregion

    public class RelayCommand : ICommand
    { 
        private readonly CanExecuteHandler<object> canExecute;
        private readonly ExecuteHandler<object> execute;
        private readonly EventHandler requerySuggested;
 
        /// <summary>Событие извещающее об изменении состояния команды.</summary>
        public event EventHandler CanExecuteChanged;
 
        /// <summary>Конструктор команды.</summary>
        /// <param name="execute">Выполняемый метод команды.</param>
        /// <param name="canExecute">Метод, возвращающий состояние команды.</param>
        public RelayCommand(ExecuteHandler<object> execute, CanExecuteHandler<object> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? (obj => true);
        }
 
        /// <inheritdoc cref="RelayCommand(ExecuteHandler{object}, CanExecuteHandler{object})"/>
        public RelayCommand(ExecuteHandler execute, CanExecuteHandler canExecute = null)
                : this
                (
                      p => execute(),
                      canExecute == null ? null : p => canExecute()
                )
        { }
 
        /// <summary>Метод, подымающий событие <see cref="CanExecuteChanged"/>.</summary>
        public virtual void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
 
        //public override void RaiseCanExecuteChanged()
        // => Device.BeginInvokeOnMainThread(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));

        /// <summary>Вызов метода, возвращающего состояние команды.</summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns><see langword="true"/> - если выполнение команды разрешено.</returns>
        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;
 
        /// <summary>Вызов выполняющего метода команды.</summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object parameter) => execute?.Invoke(parameter);
    }
 


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
