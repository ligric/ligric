namespace Common.EventArgs
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
        Cleared
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

        // Создание аргумента для события извещения о добавления в словарь новой пары ключ-значение.
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> AddKey<TKey, TValue>(TKey key, TValue value)
          => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Added, key, default, value);

        // Создание аргумента для события извещения об удалении из словаря пары ключ-значение.
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> RemoveKey<TKey, TValue>(TKey key, TValue value)
            => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Removed, key, value, default);

        // Создание аргумента для события извещения о замене в словаря значения ключа.
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> ChangedValue<TKey, TValue>(TKey key, TValue oldValue, TValue newValue)
            => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Changed, key, oldValue, newValue);

        // Создание аргумента для события извещения об очистке словаря (то есть удалении всех пар ключ-значение).
        public static NotifyDictionaryChangedEventArgs<TKey, TValue> Cleared<TKey, TValue>()
            => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Cleared, default, default, default);
    }
}
