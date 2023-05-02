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
				Side = dto.Side.ToOrderSideProto(),
				Symbol = dto.Symbol,
				Value = dto.Value.ToString()
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

		public static Ligric.Core.Types.OrderSide ToOrderSideDto(this Ligric.Protobuf.OrderSide sideInput)
		{
			switch (sideInput)
			{
				case Ligric.Protobuf.OrderSide.Sell:
					return Ligric.Core.Types.OrderSide.Sell;
				case Ligric.Protobuf.OrderSide.Buy:
					return Ligric.Core.Types.OrderSide.Buy;
			}
			throw new NotImplementedException();
		}

		public static Ligric.Protobuf.OrderSide ToOrderSideProto(this Ligric.Core.Types.OrderSide sideInput)
		{
			switch (sideInput)
			{
				case Ligric.Core.Types.OrderSide.Sell:
					return Ligric.Protobuf.OrderSide.Sell;
				case Ligric.Core.Types.OrderSide.Buy:
					return Ligric.Protobuf.OrderSide.Buy;
			}
			throw new NotImplementedException();
		}

		public static Ligric.Core.Types.PositionSide ToPositionSideDto(this Ligric.Protobuf.PositionSide sideInput)
		{
			switch (sideInput)
			{
				case Ligric.Protobuf.PositionSide.Short:
					return Ligric.Core.Types.PositionSide.Short;
				case Ligric.Protobuf.PositionSide.Long:
					return Ligric.Core.Types.PositionSide.Long;
				case Ligric.Protobuf.PositionSide.Both:
					return Ligric.Core.Types.PositionSide.Both;
			}
			throw new NotImplementedException();
		}

		public static Ligric.Protobuf.PositionSide ToPositionSide(this Ligric.Core.Types.PositionSide sideInput)
		{
			switch (sideInput)
			{
				case Ligric.Core.Types.PositionSide.Short:
					return Ligric.Protobuf.PositionSide.Short;
				case Ligric.Core.Types.PositionSide.Long:
					return Ligric.Protobuf.PositionSide.Long;
				case Ligric.Core.Types.PositionSide.Both:
					return Ligric.Protobuf.PositionSide.Both;
			}
			throw new NotImplementedException();
		}

		public static FuturesPosition ToFuturesPosition(this FuturesPositionDto dto)
		{
			return new FuturesPosition
			{
				Id = dto.Id,
				Symbol = dto.Symbol,
				Side = dto.Side.ToPositionSide(),
				EntryPrice = dto.EntryPrice.ToString()
			};
		}
	}
}
