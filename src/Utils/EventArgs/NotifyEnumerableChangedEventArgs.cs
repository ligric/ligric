using System.Collections.Generic;

namespace Utils
{
    public enum NotifyEnumumerableChangedAction
    {
        Added,
        Changed,
        Removed,
        Reset
    }


    /// <summary>Аргументы события изменения словаря.</summary>
    /// <typeparam name="TKey">Тип ключа словаря.</typeparam>
    /// <typeparam name="TValue">Тип значения словаря.</typeparam>
    public class NotifyEnumerableChangedEventArgs<T> : NotifyActionEnumerableChangedEventArgs
    {
        /// <summary>Удаляемые или изменяемые значения.</summary>
        public IEnumerable<T> OldValues { get; }

        /// <summary>Добавляемые или новые значение.</summary>
        public IEnumerable<T> NewValues { get; }

        /// <summary>Порядковый номер события.</summary>
        public int Number { get; }

        /// <summary>Время вызова события в форамте UNIX.</summary>
        public long SenderTime { get; }

        public NotifyEnumerableChangedEventArgs(NotifyEnumumerableChangedAction action,
                                                IEnumerable<T> oldValues, IEnumerable<T> newValues,
                                                int number, long senderTime)
            : base(action)
        {
            OldValues = oldValues;
            NewValues = newValues;

            Number = number;
            SenderTime = senderTime;
        }

        public NotifyEnumerableChangedEventArgs(NotifyEnumumerableChangedAction action,
                                                T oldValue, T newValue,
                                                int number, long senderTime)
            : this(action,
                 oldValue == null ? new T[0] : new T[] { oldValue },
                 newValue == null ? new T[0] : new T[] { newValue },
                 number, senderTime)
        { }
    }

    public class NotifyActionEnumerableChangedEventArgs : System.EventArgs
    {
        public NotifyEnumumerableChangedAction Action { get; }

        public NotifyActionEnumerableChangedEventArgs(NotifyEnumumerableChangedAction action)
        {
            Action = action;
        }

        #region Enumerable
        //Add
        public static NotifyEnumerableChangedEventArgs<T> AddedEnumerable<T>(IEnumerable<T> value, int number, long senderTime)
          => new NotifyEnumerableChangedEventArgs<T>(NotifyEnumumerableChangedAction.Added, default, value, number, senderTime);
        //Remove
        public static NotifyEnumerableChangedEventArgs<T> RemovedEnumerable<T>(IEnumerable<T> value, int number, long senderTime)
            => new NotifyEnumerableChangedEventArgs<T>(NotifyEnumumerableChangedAction.Removed, value, default, number, senderTime);
        //Replace
        public static NotifyEnumerableChangedEventArgs<T> ChangedEnumerable<T>(IEnumerable<T> oldValues, IEnumerable<T> newValues, int number, long senderTime)
            => new NotifyEnumerableChangedEventArgs<T>(NotifyEnumumerableChangedAction.Changed, oldValues, newValues, number, senderTime);
        #endregion

        #region Single
        //Add
        public static NotifyEnumerableChangedEventArgs<T> Added<T>(T value, int number, long senderTime)
          => new NotifyEnumerableChangedEventArgs<T>(NotifyEnumumerableChangedAction.Added, default, value, number, senderTime);
        //Remove
        public static NotifyEnumerableChangedEventArgs<T> Removed<T>(T value, int number, long senderTime)
            => new NotifyEnumerableChangedEventArgs<T>(NotifyEnumumerableChangedAction.Removed, value, default, number, senderTime);
        //Replace
        public static NotifyEnumerableChangedEventArgs<T> Changed<T>(T oldValues, T newValues, int number, long senderTime)
            => new NotifyEnumerableChangedEventArgs<T>(NotifyEnumumerableChangedAction.Changed, oldValues, newValues, number, senderTime);
        #endregion

        // Reset
        public static NotifyEnumerableChangedEventArgs<T> Reset<T>(IEnumerable<T> newValues, int number, long senderTime)
            => new NotifyEnumerableChangedEventArgs<T>(NotifyEnumumerableChangedAction.Reset, default, newValues, number, senderTime);
    }
}
