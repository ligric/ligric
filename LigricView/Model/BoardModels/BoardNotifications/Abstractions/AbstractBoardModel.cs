using BoardsShared.AbstractBoardNotifications.Interfaces;
using BoardsShared.CommonTypes.Entities;
using Common;
using Common.Delegates;
using Common.Enums;
using Common.EventArgs;
using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BoardsShared.AbstractBoardNotifications.Abstractions
{
    public abstract class AbstractBoardModel<TKey, TValue> : IAdDictionaryNotification<TKey, TValue>, IBoardNameNotification, IBoardFiltersNotification, IBoardStateNotification
         where TValue : AdDto
    {
        protected readonly SyncMethod syncMethod = new SyncMethod();

        protected int syncNumer = 0;

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
        private readonly Dictionary<TKey, TValue> ads = new Dictionary<TKey, TValue>();

        public IReadOnlyDictionary<TKey, TValue> Ads { get; }

        public event EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> AdsChanged;

        #region Методы для изменения словаря.
        protected void AdsRaiseActionAddValue(TKey key, TValue value)
        {
            ads.AddAndShout(this, AdsChanged, key, value, ref syncNumer);
        }

        protected void AdsRaiseActionRemoveValue(TKey key)
        {
            ads.RemoveAndShout(this, AdsChanged, key, ref syncNumer);
        }

        protected void AdsRaiseActionSetValue(TKey key, TValue value)
        {
            ads.SetAndShout(this, AdsChanged, key, value, ref syncNumer);
        }

        protected void AdsRaiseActionClear()
        {
            ads.ClearAndShout(this, AdsChanged, ref syncNumer);
        }

        protected void AdsRaiseActionInitialized(IDictionary<TKey, TValue> newValues)
        {
            AdsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.InitializeKeyValuePairs(newValues, syncNumer++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
        }
        #endregion 
        #endregion

        public AbstractBoardModel(string boardName, StateEnum state = StateEnum.Stoped)
        {
            Name = boardName;
            CurrentState = state;
            Ads = new ReadOnlyDictionary<TKey, TValue>(ads);
        }

        public AbstractBoardModel(string boardName, IDictionary<string, string> filters, StateEnum state = StateEnum.Stoped) :
            this(boardName, state)
        {
            Filters = filters;
        }
    }
}
