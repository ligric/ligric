using BoardRepositories.BitZlato.Types;
using Common.EventArgs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BoardRepositories.BitZlato
{
    public partial class BitZlatoWithTimerRepository
    {
        private event EventHandler<NotifyEnumerableChangedEventArgs<Ad>> privateAdsChanged;

        public override event EventHandler<NotifyEnumerableChangedEventArgs<Ad>> AdsChanged
        {
            add
            {
                lock (((ICollection)ads).SyncRoot)
                {
                    value?.Invoke(this, NotifyActionEnumerableChangedEventArgs.Reset(ads.Values.ToArray(), actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                    privateAdsChanged += value;
                }
            }
            remove
            {
                lock (((ICollection)ads).SyncRoot)
                {
                    privateAdsChanged -= value;
                }
            }
        }

        #region Single Ad Notifications
        protected void AdAdsDictionary(Ad ad)
        {
            if (ads.ContainsKey(ad.Id))
                return;

            lock (((ICollection)ads).SyncRoot)
            {
                ads.Add(ad.Id, ad);
                privateAdsChanged?.Invoke(this, NotifyActionEnumerableChangedEventArgs.Added(ad, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));               
            }
        }

        protected void RemoveFromAdsDictionary(long id)
        {
            lock (((ICollection)ads).SyncRoot)
            {
                if (ads.TryGetValue(id, out Ad ad))
                {
                    ads.Remove(id);
                    privateAdsChanged?.Invoke(this, NotifyActionEnumerableChangedEventArgs.Removed(ad, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                }
            }
        }

        protected void ChangeInAdsDictionary(Ad changedAd)
        {
            lock (((ICollection)ads).SyncRoot)
            {
                if (ads.TryGetValue(changedAd.Id, out Ad ent))
                {
                    if (ent == null)
                        return;

                    if (!Equals(changedAd, ent))
                    {
                        var oldAd = ent;
                        ent = changedAd;
                        privateAdsChanged?.Invoke(this, NotifyActionEnumerableChangedEventArgs.Changed(oldAd, ent, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                    }
                }
                else
                {
                    throw new ArgumentException($"Попытка изменить в списке {nameof(ads)} елемент, которого в нём нет :(\nТип класса: {nameof(BitZlatoWithTimerRepository)}\nМетод: {nameof(ChangeInAdsDictionary)}");
                }
            }
        }
        #endregion

        #region Enumerable Ads Notifications
        protected void AdAdsDictionary(IEnumerable<Ad> newAds)
        {
            List<Ad> addedAds = new List<Ad>();
            lock (((ICollection)ads).SyncRoot)
            { 
                foreach (var ad in newAds)
                {
                    if (!ads.ContainsKey(ad.Id))
                    {
                        addedAds.Add(ad);
                        ads.Add(ad.Id, ad); 
                    }
                }
                if (addedAds.Count > 0)
                    privateAdsChanged?.Invoke(this, NotifyActionEnumerableChangedEventArgs.AddedEnumerable(addedAds, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
            }
        }

        // TODO : некоторые элменты могут быть не удалены!
        protected void RemoveFromAdsDictionary(IEnumerable<long> ids)
        {
            List<Ad> deletedAds = new List<Ad>();
            lock (((ICollection)ads).SyncRoot)
            {
                foreach (var id in ids)
                {
                    if (ads.TryGetValue(id, out Ad ad))
                    {
                        deletedAds.Remove(ad);
                        ads.Remove(id);
                    }
                }
                if (deletedAds.Count > 0)
                    privateAdsChanged?.Invoke(this, NotifyActionEnumerableChangedEventArgs.RemovedEnumerable(deletedAds, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
            }
        }

        protected void ChangeInAdsDictionary(IEnumerable<Ad> changedValues)
        {
            List<Ad> changedAds = new List<Ad>();
            List<Ad> oldAds = new List<Ad>();
            lock (((ICollection)ads).SyncRoot)
            {
                foreach (var newAdValue in changedValues)
                {
                    if (ads.TryGetValue(newAdValue.Id, out Ad oldValue))
                    {
                        if (oldValue != null)
                        {
                            if (!Equals(newAdValue, oldValue))
                            {
                                var oldAd = oldValue;
                                oldValue = newAdValue;

                                oldAds.Add(oldAd);
                                changedAds.Add(newAdValue);
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"Попытка изменить в списке {nameof(ads)} елемент, которого в нём нет :(\nТип класса: {nameof(BitZlatoWithTimerRepository)}\nМетод: {nameof(ChangeInAdsDictionary)}");
                    }
                }

                if (oldAds.Count != changedAds.Count)
                    throw new ArgumentException($"Изменённые списки не сошлись :(\nТип класса: {nameof(BitZlatoWithTimerRepository)}\nМетод: {nameof(ChangeInAdsDictionary)}");         

                if (changedAds.Count > 0)
                    privateAdsChanged?.Invoke(this, NotifyActionEnumerableChangedEventArgs.ChangedEnumerable(oldAds, changedAds, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                
            }
        }
        #endregion

        protected void CommonAdsDictionaryHandler(IEnumerable<Ad> receivedAds)
        {
            List<Ad> newAds = new List<Ad>();
            List<Ad> changedAds = new List<Ad>();
            List<Ad> oldAds = new List<Ad>();
            List<Ad> noRemoveAds = new List<Ad>();
            List<Ad> removeAds = new List<Ad>();


            lock (((ICollection)ads).SyncRoot)
            {
                foreach (var ad in receivedAds)
                {
                    noRemoveAds.Add(ad);
                    if (ads.TryGetValue(ad.Id, out var ent))
                    {
                        if (!Equals(ad, ent))
                        {
                            var oldAd = ent;
                            ent = ad;

                            oldAds.Add(oldAd);
                            changedAds.Add(ad);
                        }
                    }
                    else
                    {                     
                        ads.Add(ad.Id, ad);
                        newAds.Add(ad);
                    }
                }

                if (noRemoveAds.Count > 0)
                {
                    foreach (var ad in ads)
                    {
                        if (noRemoveAds.Find(x => x.Id == ad.Key) == null)
                        {
                            ads.Remove(ad.Key);
                            removeAds.Add(ad.Value);
                        }
                    }
                    privateAdsChanged?.Invoke(this, NotifyActionEnumerableChangedEventArgs.RemovedEnumerable(removeAds, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                }

                if (changedAds.Count > 0)
                    privateAdsChanged?.Invoke(this, NotifyActionEnumerableChangedEventArgs.ChangedEnumerable(oldAds, changedAds, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));

                if (newAds.Count > 0)
                    privateAdsChanged?.Invoke(this, NotifyActionEnumerableChangedEventArgs.AddedEnumerable(newAds, actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds())); 
            }   
        }
    }
}
