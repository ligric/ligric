using Common.Enums;

namespace Common.DtoTypes.Board
{
    readonly public struct AdReadOnlyStruct
    {
        public long Id { get; }
        public TraderReadOnlyStruct Trader { get; }
        public PaymethodReadOnlyStruct Paymethod { get; }
        public RateReadOnlyStruct Rate { get; }
        public LimitReadOnlyStruct Limit { get; }
        public AdTypeEnum Type { get; }

        public AdReadOnlyStruct(long id, TraderReadOnlyStruct trader, PaymethodReadOnlyStruct paymethod, 
                                   RateReadOnlyStruct rate, LimitReadOnlyStruct limit, AdTypeEnum type)
        {
            Id = id; Trader = trader; Paymethod = paymethod; Rate = rate; Limit = limit; Type = type;
        }
    }
}
