using Ligric.Application.Configuration.Commands;

namespace TemporaryProjectJustForCopyPast.Application.UserApis.CreateUserApi
{
	public class CreateUserApiCommand : CommandBase<long>
	{
		public string Name { get; }

		public long OwnerId { get; }

		public string PrivateKey { get; }

		public string PublicKey { get; }

		/// <summary>
		/// Flag value: <see cref="Ligric.Core.Ligric.Core.Types.Api.ApiPermissions"/>
		/// </summary>
		public int Permissions { get; }

		public CreateUserApiCommand(
			string name,
			long ownerId,
			string privateKey,
			string publicKey,
			int permissions)
		{
			Name = name;
			OwnerId = ownerId;
			PrivateKey = privateKey;
			PublicKey = publicKey;
			Permissions = permissions;
		}
	}
}
