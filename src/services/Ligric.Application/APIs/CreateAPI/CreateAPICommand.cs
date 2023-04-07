using Ligric.Application.Configuration.Commands;
using Ligric.Core.Ligric.Core.Types.Api;

namespace Ligric.Application.Users.RegisterUser
{
	public class CreateAPICommand : CommandBase<int>
    {
		public string Name { get; }

		public string PrivateKey { get; }

		public string PublicKey { get; }

		public CreateAPICommand(string name, string privateKey, string publicKey)
        {
			Name = name;
			PrivateKey = privateKey;
			PublicKey = publicKey;
        }
    }
}
