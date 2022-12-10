namespace Ligric.Common.Types.Api
{
    public class ApiDto
    {
        public long? Id { get; }

        public string Name { get; }

        public string PublicKey { get; }

        public string PrivateKey { get; }

        public ApiDto(long? id, string name, string publicKey, string privateKey)
        {
            Id = id;
            Name = name;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
    }
}
