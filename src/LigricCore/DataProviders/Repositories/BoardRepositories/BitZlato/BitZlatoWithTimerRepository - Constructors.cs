using BitZlatoApi;
using BoardRepositories.Interfaces;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace BoardRepositories.BitZlato
{
    public partial class BitZlatoWithTimerRepository
    {
        public BitZlatoWithTimerRepository(string apiKey, string email, StateEnum defaultState = StateEnum.Stoped)
            : base(defaultState)
        {
            bitZlatoApi = new BitZlatoRequests(apiKey, email);
            ((ISupportInitializeBoardRepository)this).Initialize(defaultState);
        }

        public BitZlatoWithTimerRepository(string apiKey, string email, IDictionary<string, string> parameters, StateEnum defaultState = StateEnum.Stoped)
           : base(parameters, defaultState)
        {
            bitZlatoApi = new BitZlatoRequests(apiKey, email);
            ((ISupportInitializeBoardRepository)this).Initialize(parameters, defaultState);
        }

        public BitZlatoWithTimerRepository(string apiKey, string email, TimeSpan timerInterval, StateEnum defaultState = StateEnum.Stoped)
            : base(timerInterval, defaultState)
        {
            bitZlatoApi = new BitZlatoRequests(apiKey, email);
            ((ISupportInitializeBoardRepository)this).Initialize(defaultState);
        }

        public BitZlatoWithTimerRepository(string apiKey, string email, TimeSpan timerInterval, IDictionary<string, string> parameters, StateEnum defaultState = StateEnum.Stoped)
            : base(timerInterval, parameters, defaultState)
        {
            bitZlatoApi = new BitZlatoRequests(apiKey, email);
            ((ISupportInitializeBoardRepository)this).Initialize(parameters, defaultState);
        }
    }
}
