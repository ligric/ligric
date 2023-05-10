using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Core.Types.Future;
using Ligric.Protobuf;
using Ligric.Service.CryptoApisService.Domain.Entities;

namespace Ligric.Service.CryptoApisService.Api.Extensions
{
	public static class TypeExtensions
	{
		public static FuturesOrder ToFutureOrder(this FuturesOrderDto dto)
		{
			return new FuturesOrder
			{
				Id = dto.Id,
				Price = dto.Price.ToString(),
				Quantity = dto.Quantity.ToString(),
				Side = dto.Side.ToSideProto(),
				Symbol = dto.Symbol,
				CurrentPrice = dto.CurrentPrice.ToString(),
				Type = dto.Type.ToString()
			};
		}

		public static UserApiEntity ToUserApiEntity(this ApiClientDto userApi)
		{
			return new UserApiEntity
			{
				Id = userApi.UserApiId,
				Name = userApi.Name,
				Permissions = userApi.Permissions
			};
		}

		public static ApiDto ToApiDto(this ApiEntity entity)
		{
			return new ApiDto(entity.Id ?? throw new ArgumentNullException("Api id is null"),
				entity.PublicKey ?? throw new ArgumentNullException("Api public key is null"),
				entity.PrivateKey ?? throw new ArgumentNullException("Api private key is null"));
		}

		public static Ligric.Core.Types.Side ToSideDto(this Ligric.Protobuf.Side sideInput)
		{
			switch (sideInput)
			{
				case Ligric.Protobuf.Side.Sell:
					return Ligric.Core.Types.Side.Sell;
				case Ligric.Protobuf.Side.Buy:
					return Ligric.Core.Types.Side.Buy;
			}
			throw new NotImplementedException();
		}

		public static Ligric.Protobuf.Side ToSideProto(this Ligric.Core.Types.Side sideInput)
		{
			switch (sideInput)
			{
				case Ligric.Core.Types.Side.Sell:
					return Ligric.Protobuf.Side.Sell;
				case Ligric.Core.Types.Side.Buy:
					return Ligric.Protobuf.Side.Buy;
			}
			throw new NotImplementedException();
		}

		public static Ligric.Protobuf.Side ToSide(this Ligric.Core.Types.Side sideInput)
		{
			switch (sideInput)
			{
				case Ligric.Core.Types.Side.Sell:
					return Ligric.Protobuf.Side.Sell;
				case Ligric.Core.Types.Side.Buy:
					return Ligric.Protobuf.Side.Buy;
			}
			throw new NotImplementedException();
		}

		public static FuturesPosition ToFuturesPosition(this FuturesPositionDto dto)
		{
			return new FuturesPosition
			{
				Id = dto.Id,
				Symbol = dto.Symbol,
				Side = dto.Side.ToSide(),
				EntryPrice = dto.EntryPrice.ToString(),
				Quantity = dto.Quantity.ToString(),
			};
		}
	}
}
