using AbstractionBoardRepository.Interfaces;
using Common.DtoTypes.Board;
using Common.EventArgs;
using System.Collections.ObjectModel;

namespace AbstractionBoardRepository.Abstracts
{
    public abstract class AbstractAdBoardNotifications : IRepositoryStateNotification, IAdBoardDictionaryNotification, IAdBoardNameNotification, IAdBoardFiltersNotification
    {
        #region RepositoryStateChanged
        public RepositoryStateEnum CurrentRepositoryState { get; private set; }
        public event ActionRepositoryStateHandler RepositoryStateChanged;

        public virtual bool SetAdBoardStateAndSendAction(RepositoryStateEnum repositoryState)
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
        private readonly IDictionary<long, AdDto> ads = new Dictionary<long, AdDto>();
        public IReadOnlyDictionary<long, AdDto> Ads { get; }

        public event EventHandler<NotifyDictionaryChangedEventArgs<long, AdDto>> AdsChanged;

        #region Методы для изменения словаря.
        protected bool AddAd(long id, AdDto ad)
        {
            if (ads.ContainsKey(id))
                return false;

            ads.Add(id, ad);
            AdsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.AddKey(id, ad));
            return true;
        }

        protected bool RemoveAd(long id)
        {
            if (ads.TryGetValue(id, out AdDto ad))
            {
                ads.Remove(id);
                AdsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.RemoveKey(id, ad));
                return true;
            }
            return false;

        }

        protected bool ChangeAd(long id, AdDto newAd)
        {
            if (ads.TryGetValue(id, out AdDto ent))
            {
                if (!Equals(newAd, ent))
                {
                    var oldAd = ent;
                    ent = newAd;
                    AdsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.ChangedValue(newAd.Id, oldAd, ent));
                }
                return true;
            }
            return false;
        }

        protected bool NewAdsHandler(IEnumerable<AdDto> receivedAds)
        {
            foreach (var newAd in receivedAds)
            {
                if(!ChangeAd(newAd.Id, newAd))
                {
                    ads.Add(newAd.Id, newAd);
                    AdsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.AddKey(newAd.Id, newAd));
                }
            }

            return false;
        }
        #endregion

        #endregion

        /*------------------------------------------------------------------------------------------------------------------*/
        #region Constructors
        protected AbstractAdBoardNotifications()
        {
            Ads = new ReadOnlyDictionary<long, AdDto>(ads);
            Filters = new Dictionary<string, string>();
            CurrentRepositoryState = RepositoryStateEnum.Stoped;
        }

        protected AbstractAdBoardNotifications(IDictionary<string, string> filters)
        {
            Ads = new ReadOnlyDictionary<long, AdDto>(ads);
            Filters = filters;
            CurrentRepositoryState = RepositoryStateEnum.Stoped;
        }

        protected AbstractAdBoardNotifications(IDictionary<string, string> filters, RepositoryStateEnum repositoryState)
        {
            Ads = new ReadOnlyDictionary<long, AdDto>(ads);
            Filters = filters;
            CurrentRepositoryState = repositoryState;
        }
        #endregion
    }
}
