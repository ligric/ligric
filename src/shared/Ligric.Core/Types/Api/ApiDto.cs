namespace Ligric.Core.Ligric.Core.Types.Api
{
    public record ApiDto
    {
        public long Id { get; init; }

        public string PublicKey { get; init; }

		public string PrivateKey { get; init; }

		public ApiDto(long id, string publicKey, string privateKey)
        {
            Id = id;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
    }
}
