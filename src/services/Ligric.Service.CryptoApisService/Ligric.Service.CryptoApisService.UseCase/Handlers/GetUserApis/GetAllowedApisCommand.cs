using System.Collections.Generic;
using Ligric.Service.CryptoApisService.Domain.Entities;
using Ligric.Service.CryptoApisService.Infrastructure.Persistence.Commands;

namespace Ligric.Service.CryptoApisService.UseCase.Handlers.GetUserApis
{
	public class GetAllowedApisCommand : CommandBase<IEnumerable<UserApiEntity>>
    {
		public long UserId { get; }

		public GetAllowedApisCommand(long userId)
        {
			UserId = userId;
        }
    }
}
