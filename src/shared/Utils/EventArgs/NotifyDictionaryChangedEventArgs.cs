using System.Collections.Generic;

namespace Utils
{
    public enum NotifyDictionaryChangedAction
    {
        /// <summary>Добавлен новый ключ.</summary>
        Added,
        /// <summary>Удалён ключ.</summary>
        Removed,
        /// <summary>Изменено значение для ключа.</summary>
        Changed,
        ///<summary>Очищен словарь - все ключи удалены.</summary>
        Cleared,
        ///<summary>Словарь изменён польностью.</summary>
        Initialized
    }

    /// <summary>Аргументы события изменения словаря.</summary>
    /// <typeparam name="TKey">Тип ключа словаря.</typeparam>
    /// <typeparam name="TValue">Тип значения словаря.</typeparam>
    public class NotifyDictionaryChangedEventArgs<TKey, TValue> : NotifyActionDictionaryChangedEventArgs
    {
        /// <summary>Ключ в отношении которого произведено действие.</summary>
        public TKey? Key { get; }

        /// <summary>Удаляемое или измененое значение.</summary>
        public TValue? OldValue { get; }

        /// <summary>Добавленное или новое значение.</summary>
        public TValue? NewValue { get; }

        /// <summary>Порядковый номер события.</summary>
        public int Number { get; }

        /// <summary>Время вызова события в форамте UNIX.</summary>
        public long SenderTime { get; }

        public IDictionary<TKey, TValue>? NewDictionary { get; }

        public NotifyDictionaryChangedEventArgs(
            NotifyDictionaryChangedAction action, 
            TKey? key, 
            TValue? oldValue, 
            TValue? newValue, 
            int number, 
            long senderTime)
            : base(action)
        {
            Key = key;
            OldValue = oldValue;
            NewValue = newValue;

            Number = number;
            SenderTime = senderTime;
        }

        public NotifyDictionaryChangedEventArgs(
            NotifyDictionaryChangedAction action, 
            IDictionary<TKey, TValue>? newDictionary, 
            int number, 
            long senderTime)
            : base(action)
        {
            NewDictionary = newDictionary;

            Number = number;
            SenderTime = senderTime;
        }
    }

    /// <summary>Аргументы события изменения словаря. 
    /// Содержит только действие и метод создания экземпляров производных классов.</summary>
    public class NotifyActionDictionaryChangedEventArgs : System.EventArgs
    {
        /// <summary>Действие изменившее словарь.</summary>
        public NotifyDictionaryChangedAction Action { get; }

        /// <summary>Создаёт экземпляр.</summary>
        /// <param name="action">Действие изменившее словарь.</param>
        public NotifyActionDictionaryChangedEventArgs(NotifyDictionaryChangedAction action)
        {
            Action = action;
        }

        // Создание аргумента для события извещения о добавления в словарь новой пары ключ-значение.
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> AddKeyValuePair<TKey, TValue>(TKey key, TValue value, int number, long senderTime)
          => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Added, key, default, value, number, senderTime);

        // Создание аргумента для события извещения об удалении из словаря пары ключ-значение.
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> RemoveKeyValuePair<TKey, TValue>(TKey key, int number, long senderTime, TValue? oldValue = default)
            => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Removed, key, oldValue, default, number, senderTime);

        // Создание аргумента для события извещения о замене в словаря значения ключа.
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> ChangeKeyValuePair<TKey, TValue>(TKey key, TValue oldValue, TValue newValue, int number, long senderTime)
            => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Changed, key, oldValue, newValue, number, senderTime);

        // Создание аргумента для события извещения о полной замене всех элементов словаря.
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> InitializeKeyValuePairs<TKey, TValue>(IDictionary<TKey, TValue> values, int number, long senderTime)
            => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Initialized, values, number, senderTime);

        // Создание аргумента для события извещения об очистке словаря (то есть удалении всех пар ключ-значение).
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> ClearKeyValuePairs<TKey, TValue>(int number, long senderTime)
            => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Cleared, default, default, default, number, senderTime);
    }
}
