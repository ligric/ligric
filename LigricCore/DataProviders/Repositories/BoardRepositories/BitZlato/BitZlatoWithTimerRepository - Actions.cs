using BoardRepositories.BitZlato.Types;
using BoardRepositories.Interfaces;
using Common.EventArgs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BoardRepositories.BitZlato
{
    public partial class BitZlatoWithTimerRepository : IBoardRepository<long, Ad>
    {
        private int actionNumber = 0;
        private event EventHandler<NotifyDictionaryChangedEventArgs<long, Ad>> privateAdsChanged;

        public override event EventHandler<NotifyDictionaryChangedEventArgs<long, Ad>> AdsChanged
        {
            add
            {
                lock (((ICollection)ads).SyncRoot)
                {
                    value?.Invoke(this, NotifyActionDictionaryChangedEventArgs.InitializeKeyValuePairs(new Dictionary<long, Ad>(ads), actionNumber++, DateTimeOffset.Now.ToUnixTimeMilliseconds()));
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
    }
}
