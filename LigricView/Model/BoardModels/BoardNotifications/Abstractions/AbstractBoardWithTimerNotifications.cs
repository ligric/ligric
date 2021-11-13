using BoardModels.AbstractBoardNotifications.Interfaces;
using BoardModels.BoardNotifications.Interfaces;
using BoardModels.CommonTypes.Entities;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace BoardModels.AbstractBoardNotifications.Abstractions
{
    public abstract class AbstractBoardWithTimerNotifications<TKey, TValue> : AbstractBoardModel<TKey, TValue>, IBoardNameNotification, IBoardFiltersNotification
        where TValue : AdDto
    {
        public AbstractBoardWithTimerNotifications(string boardName, TimeSpan interval, IDictionary<string, string> filters, StateEnum defaultState = StateEnum.Stoped)
            : base(boardName, filters, defaultState)
        {

        }

        public event ActionTimerIntervalHandler TimerIntervalChanged;



        //public AbstractBoardWithTimerNotifications(IDictionary<string, string> parameters, StateEnum defaultState = StateEnum.Stoped)
        //   : base(parameters, defaultState)
        //{
        //    bitZlatoApi = new BitZlatoRequests(apiKey, email);
        //    ((ISupportInitializeBoardRepository)this).Initialize(parameters, defaultState);
        //}

        //public AbstractBoardWithTimerNotifications(string apiKey, string email, TimeSpan timerInterval, StateEnum defaultState = StateEnum.Stoped)
        //    : base(timerInterval, defaultState)
        //{
        //    bitZlatoApi = new BitZlatoRequests(apiKey, email);
        //    ((ISupportInitializeBoardRepository)this).Initialize(defaultState);
        //}

        //public AbstractBoardWithTimerNotifications(string apiKey, string email, TimeSpan timerInterval, IDictionary<string, string> parameters, StateEnum defaultState = StateEnum.Stoped)
        //    : base(timerInterval, parameters, defaultState)
        //{
        //    bitZlatoApi = new BitZlatoRequests(apiKey, email);
        //    ((ISupportInitializeBoardRepository)this).Initialize(parameters, defaultState);
        //}
    }
}
