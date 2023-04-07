using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ligric.Service.CryptoApisService.Infrastructure.Persistence.Commands;

namespace Ligric.Service.CryptoApisService.Infrastructure.Handlers.ShareUserApi
{
	public class ShareUserApiCommand : CommandBase<bool>
	{
		public long UserApiId { get; }

		public int Permissions { get; }

		public ReadOnlyCollection<long> UserIds { get; }

		public ShareUserApiCommand(long userApiId, int permissions, IList<long> userIds)
		{
			UserApiId = userApiId;
			Permissions = permissions;
			UserIds = new ReadOnlyCollection<long>(userIds);
		}
	}
}
