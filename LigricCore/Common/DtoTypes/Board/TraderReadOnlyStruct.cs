namespace Common.DtoTypes.Board
{
    readonly public struct TraderReadOnlyStruct
    {
        public ushort Id { get; }
        public string Name { get; }

        public TraderReadOnlyStruct(ushort id, string name)
        {
            Id = id; Name = name;
        }
    }
}