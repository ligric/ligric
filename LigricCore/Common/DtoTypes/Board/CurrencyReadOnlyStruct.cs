using Common.Enums;

namespace Common.DtoTypes.Board
{
    readonly public struct CurrencyReadOnlyStruct
    {
        public byte Id { get; }
        public string Name { get; }
        public string Symbol { get; }
        public CurrencyTypeEnum Type { get; }
        public CurrencyReadOnlyStruct(byte id, string name, string symbol, CurrencyTypeEnum type)
        {
            Id = id; Name = name; Symbol = symbol; Type = type;
        }
    }
}
