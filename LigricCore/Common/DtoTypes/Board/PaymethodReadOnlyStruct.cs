namespace Common.DtoTypes.Board
{
    readonly public struct PaymethodReadOnlyStruct
    {
        public ushort Id { get; }
        public string Name { get; }
        public PaymethodReadOnlyStruct(ushort id, string name)
        {
            Id = id; Name = name;
        }
    }
}