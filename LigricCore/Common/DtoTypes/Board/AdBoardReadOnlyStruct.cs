namespace Common.DtoTypes.Board
{
    readonly public struct AdBoardReadOnlyStruct
    {
        public byte Id { get; }
        public AdReadOnlyStruct Ad { get; }

        public AdBoardReadOnlyStruct(byte id, AdReadOnlyStruct ad)
        {
            Id = id; Ad = ad;
        }
    }

}
