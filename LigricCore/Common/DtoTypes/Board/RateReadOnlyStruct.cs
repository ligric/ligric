namespace Common.DtoTypes.Board
{
    readonly public struct RateReadOnlyStruct
    {
        public CurrencyReadOnlyStruct LeftCurrency { get; }
        public CurrencyReadOnlyStruct RightCurrency { get; }
        public decimal Value { get; }

        public RateReadOnlyStruct(CurrencyReadOnlyStruct leftCurrency, CurrencyReadOnlyStruct rightCurrency, decimal value)
        {
            LeftCurrency = leftCurrency; RightCurrency = rightCurrency; Value = value;
        }
    }
}