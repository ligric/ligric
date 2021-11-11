﻿using Common.EventArgs;
using System;
using System.Collections.Generic;
using BoardRepositories.Interfaces;
using Common.Delegates;
using Common.Enums;

namespace BoardRepositories.Abstractions
{
    public abstract partial class AbstractBoardRepository<T>
    {
        public abstract event EventHandler<NotifyEnumerableChangedEventArgs<T>> AdsChanged;

        public event ActionStateHandler StateChanged;
        protected void RaiseActionState(StateEnum state)
            => StateChanged?.Invoke(this, state);


        public event ActionParametersResetHandler ParametersChanged;
        protected void RaiseActionParameters(IDictionary<string, string> newParameters)
            => ParametersChanged?.Invoke(this, newParameters);

    }
}
