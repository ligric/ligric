namespace Common.EventArgs
{
    public enum NotifyDictionaryChangedAction
    {
        /// <summary>Добавлен новый ключ.</summary>
        Added,
        /// <summary>Удалён ключ.</summary>
        Removed,
        /// <summary>Изменено значение для ключа.</summary>
        Changed
    }
    /// <summary>Аргументы события изменения словаря.</summary>
    /// <typeparam name="TKey">Тип ключа словаря.</typeparam>
    /// <typeparam name="TValue">Тип значения словаря.</typeparam>
    public class NotifyDictionaryChangedEventArgs<TKey, TValue> : NotifyActionDictionaryChangedEventArgs
    {

        /// <summary>Ключ в отношении которого произведено действие.</summary>
        public TKey Key { get; }

        /// <summary>Удаляемое или измененое значение.</summary>
        public TValue OldValue { get; }

        /// <summary>Добавленное или новое значение.</summary>
        public TValue NewValue { get; }

        public NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction action, TKey key, TValue oldValue, TValue newValue)
            : base(action)
        {
            Key = key;
            OldValue = oldValue;
            NewValue = newValue;
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

        public static NotifyDictionaryChangedEventArgs<TKey, TValue> AddKey<TKey, TValue>(TKey key, TValue value)
          => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Added, key, default, value);
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> RemoveKey<TKey, TValue>(TKey key, TValue value)
            => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Removed, key, value, default);
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> ChangedValue<TKey, TValue>(TKey key, TValue oldValue, TValue newValue)
            => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Changed, key, oldValue, newValue);
    }
}
