﻿using Newtonsoft.Json;

namespace BitZlatoApi.Types
{
    public abstract class ResponseAbstract<T>
    {
        public bool Success { get; protected set; }
    }

    public class ResponseJson<T> : ResponseAbstract<T>
    {
        private string _error;
        [JsonProperty("error")]
        public string Error
        {
            get => _error;
            set
            {
                Success = false;
                _error = value;
            }
        }

        private string _message;
        [JsonProperty("message")]
        public string Message
        {
            get => _message;
            set
            {
                Success = false;
                _message = value;
            }
        }

        private long _total;
        [JsonProperty("total")]
        public long Total
        {
            get => _total;
            set
            {
                Success = true;
                _total = value;
            }
        }

        private T _data;
        [JsonProperty("data")]
        public T Data 
        {
            get => _data;
            set 
            {
                Success = true;
                _data = value;
            }
        }
    }
}