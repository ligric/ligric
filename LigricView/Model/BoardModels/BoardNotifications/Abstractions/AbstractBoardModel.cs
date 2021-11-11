using BoardModels.AbstractBoardNotifications.Interfaces;
using BoardModels.CommonTypes.Entities;
using Common.Delegates;
using Common.Enums;
using Common.EventArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BoardModels.AbstractBoardNotifications.Abstractions
{
    public abstract class AbstractBoardModel<T> : IAdDictionaryNotification<T>, IBoardNameNotification, IBoardFiltersNotification, IBoardStateNotification
         where T : AdDto
    {
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

        #region BoardStateChanged
        public StateEnum CurrentState { get; private set; }
        public event ActionStateHandler BoardStateChanged;

        protected bool SetRepositoryStateAndSendAction(StateEnum repositoryState)
        {
            if (repositoryState == CurrentState)
                return false;

            CurrentState = repositoryState;
            BoardStateChanged?.Invoke(this, repositoryState);

            return true;
        }
        #endregion

        #region AdsChanged
        private readonly Dictionary<long, T> ads = new Dictionary<long, T>();
        public IReadOnlyDictionary<long, T> Ads { get; }

        public event EventHandler<NotifyDictionaryChangedEventArgs<long, T>> AdsChanged;

        #region Методы для изменения словаря.
        ///<summary>Добавления в словарь новой пары: ключ-значение.
        /// Возвращает false, если такой ключ уже есть и добавление не было выполнено.</summary>
        protected bool AddAdAndSendAction(long id, T ad)
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
            if (ads.TryGetValue(id, out T ad))
            {
                ads.Remove(id);
                AdsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.RemoveKey(id, ad));
                return true;
            }
            return false;

        }

        ///<summary> Задание в словаре значения ключу.
        /// Возвращает false, если такого ключа нет и вместо замены было выполнено добавление.</summary>
        protected bool SetAdAndSendAction(long id, T ad)
        {
            if (ads.TryGetValue(id, out T oldAd))
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
                AdsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.Cleared<long, T>());
                return true;
            }
            return notEmpty;
        }
        #endregion

        #endregion

        public AbstractBoardModel(string boardName, StateEnum state = StateEnum.Stoped)
        {
            Name = boardName;
            CurrentState = state;
            Ads = new ReadOnlyDictionary<long,T>(ads);
        }

        public AbstractBoardModel(string boardName, IDictionary<string, string> filters, StateEnum state = StateEnum.Stoped) :
            this(boardName, state)
        {
            Filters = filters;
        }
    }
}
