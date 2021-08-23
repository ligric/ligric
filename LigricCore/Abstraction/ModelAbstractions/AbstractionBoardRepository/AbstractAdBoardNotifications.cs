using Common.DtoTypes.Board;
using System.Collections.ObjectModel;

namespace AbstractionBoardRepository
{
    public abstract class AbstractAdBoardNotifications : IAdBoardRepositoryStateNotification, IAdBoardDictionaryNotification, IAdBoardNameNotification, IAdBoardFiltersNotification
    {
        #region RepositoryStateChanged
        public RepositoryStateEnum CurrentRepositoryState { get; private set; }
        public event ActionRepositoryStateHandler RepositoryStateChanged;

        public bool SetAdBoardStateAndSendAction(RepositoryStateEnum repositoryState)
        {
            if (repositoryState == CurrentRepositoryState)
                return false;

            CurrentRepositoryState = repositoryState;
            RepositoryStateChanged?.Invoke(this, repositoryState);

            return true;
        }
        #endregion

        #region FiltersChanged
        public IDictionary<string, string> Filters { get; private set; }
        public event ActionFiltersHandler FiltersChanged;
        protected bool SetFiltersAndSendAction(IDictionary<string, string> newFiltres)
        {
            if (Filters.Count == newFiltres.Count && !Filters.Except(newFiltres).Any())
                return false;

            Filters = newFiltres;
            FiltersChanged?.Invoke(this, newFiltres);

            return true;
        }
        #endregion

        #region NameChanged
        public string Name { get; private set; }

        public event ActionNameHandler NameChanged;

        public bool SetNameAndSendAction(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            if (Name == name)
                return false;

            Name = name;
            NameChanged?.Invoke(this, name);

            return true;
        }
        #endregion

        #region AdsChanged
        private readonly IDictionary<long, AdReadOnlyStruct> ads = new Dictionary<long, AdReadOnlyStruct>();
        public IReadOnlyDictionary<long, AdReadOnlyStruct> Ads { get; }

        public event EventHandler<NotifyDictionaryChangedEventArgs<long, AdReadOnlyStruct>> AdsChanged;

        #region Методы для изменения словаря.
        ///<summary>Добавления в словарь новой пары: ключ-значение.
        /// Возвращает false, если такой ключ уже есть и добавление не было выполнено.</summary>
        protected bool AddAdAndSendAction(long id, AdReadOnlyStruct ad)
        {
            if (ads.ContainsKey(id))
                return false;

            ads.Add(id, ad);
            AdsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.AddKey(id, ad));
            return true;
        }

        ///<summary>Удаление из словаря пары: ключ-значение.
        /// Возвращает false, если такого ключа нет и удаление не было выполнено.</summary>
        protected bool RemoveAdAndSendAction(long id)
        {
            if (ads.TryGetValue(id, out AdReadOnlyStruct ad))
            {
                ads.Remove(id);
                AdsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.RemoveKey(id, ad));
                return true;
            }
            return false;

        }

        ///<summary> Задание в словаре значения ключу.
        /// Возвращает false, если такого ключа нет и вместо замены было выполнено добавление.</summary>
        protected bool SetAdAndSendAction(long id, AdReadOnlyStruct ad)
        {
            if (ads.TryGetValue(id, out AdReadOnlyStruct oldAd))
            {
                ads[id] = ad;
                AdsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.ChangedValue(id, oldAd, ad));
                return true;
            }

            ads.Add(id, ad);
            AdsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.AddKey(id, ad));
            return false;
        }

        ///<summary>Очистка словаря.
        /// Возвращает false, если словарь был пустой.</summary>
        protected bool ClearAdsAndSendAction()
        {
            var notEmpty = ads.Count != 0;

            if (notEmpty)
            {
                ads.Clear();
                AdsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.Cleared<long, AdReadOnlyStruct>());
                return true;
            }
            return notEmpty;
        }
        #endregion

        #endregion

        /// <summary>Инициализация оболочки <see cref="Ads"/>.</summary>
        protected AbstractAdBoardNotifications()
        {
            Ads = new ReadOnlyDictionary<long, AdReadOnlyStruct>(ads);
        }
    }
}
