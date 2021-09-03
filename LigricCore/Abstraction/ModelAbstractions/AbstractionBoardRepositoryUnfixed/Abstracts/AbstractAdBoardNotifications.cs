using Common.DtoTypes.Board;
using Common.EventArgs;
using System.Collections;

namespace AbstractionBoardRepository.Abstracts
{
    public abstract class BitZlatoBoardRepository
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

        #region AdsChanged
        private readonly Dictionary<long, AdDto> ads = new Dictionary<long, AdDto>();

        private event EventHandler<NotifyEnumerableChangedEventArgs<AdDto>> privateStudentsChanged;

        private static int actionNumber = 0;

        public event EventHandler<NotifyEnumerableChangedEventArgs<AdDto>> AdsChanged
        {
            add
            {
                lock (((ICollection)ads).SyncRoot)
                {                 
                    value?.Invoke(this, NotifyActionDictionaryChangedEventArgs.Reset(ads.Values.ToArray(), actionNumber++, DateTimeOffset.Now.ToUnixTimeSeconds()));
                    privateStudentsChanged += value;
                }
            }
            remove
            {
                lock (((ICollection)ads).SyncRoot)
                {
                    privateStudentsChanged -= value;
                }
            }
        }



        #region Синхронные методы для изменения словаря.
        protected bool AddAd(long id, AdDto ad)
        {
            if (ads.ContainsKey(id))
                return false;

            ads.Add(id, ad);
            privateStudentsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.Added(ad, actionNumber++, DateTimeOffset.Now.ToUnixTimeSeconds()));
            return true;
        }

        protected bool RemoveAd(long id)
        {
            if (ads.TryGetValue(id, out AdDto ad))
            {
                ads.Remove(id);
                privateStudentsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.Removed(ad, actionNumber++, DateTimeOffset.Now.ToUnixTimeSeconds()));
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
                    privateStudentsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.Changed(oldAd, ent, actionNumber++, DateTimeOffset.Now.ToUnixTimeSeconds()));
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
                    privateStudentsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.Added(newAd, actionNumber++, DateTimeOffset.Now.ToUnixTimeSeconds()));
                }
            }

            return false;
        }
        #endregion

        #endregion

        /*------------------------------------------------------------------------------------------------------------------*/
        #region Constructors
        protected BitZlatoBoardRepository()
        {
            Filters = new Dictionary<string, string>();
            CurrentRepositoryState = RepositoryStateEnum.Stoped;
        }

        protected BitZlatoBoardRepository(IDictionary<string, string> filters)
        {
            Filters = filters;
            CurrentRepositoryState = RepositoryStateEnum.Stoped;
        }

        protected BitZlatoBoardRepository(IDictionary<string, string> filters, RepositoryStateEnum repositoryState)
        {
            Filters = filters;
            CurrentRepositoryState = repositoryState;
        }
        #endregion
    }
}
