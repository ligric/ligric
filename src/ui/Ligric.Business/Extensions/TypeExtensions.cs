using System.ComponentModel;
using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.Core.Types;
using Ligric.Core.Types.Future;
using Ligric.Protobuf;

namespace Ligric.Business.Extensions
{
	internal static class TypeExtensions
	{
		public static ApiClientDto ToApiClientDto(this ApiClient apiClient)
		{
			return new ApiClientDto(apiClient.Id, apiClient.Name, apiClient.Permissions);
		}

		public static FuturesOrderDto ToFuturesOrderDto(this FuturesOrder futureOrder)
		{
			if (!Enum.TryParse(futureOrder.Type, true, out OrderType type)) throw new InvalidEnumArgumentException(futureOrder.Type);
			return new FuturesOrderDto(
				futureOrder.Id, futureOrder.Symbol, futureOrder.Side.ToSideDto(), futureOrder.PositionSide.ToPositionSideDto(),
				decimal.Parse(futureOrder.Quantity), decimal.Parse(futureOrder.Price),
				!string.IsNullOrEmpty(futureOrder.CurrentPrice) ? decimal.Parse(futureOrder.CurrentPrice) : null,
				type,
				decimal.Parse(futureOrder.StopPrice));
		}

		public static LeverageDto ToFuturesLeverageDto(this FuturesLeverage futureLeverage)
		{
			return new LeverageDto(futureLeverage.Symbol, byte.Parse(futureLeverage.Value));
		}

		public static FuturesPositionDto ToFuturesPositionDto(this FuturesPosition futuresPosition)
		{
			return new FuturesPositionDto(
				futuresPosition.Id, futuresPosition.Symbol, futuresPosition.Side.ToSideDto(),
				decimal.Parse(futuresPosition.Quantity), decimal.Parse(futuresPosition.EntryPrice),
				byte.TryParse(futuresPosition.Leverage, out byte leverageOut) ? leverageOut : null);
		}

		public static Core.Types.Side ToSideDto(this Protobuf.Side sideInput)
			 => sideInput switch
			 {
				 Protobuf.Side.Sell => Core.Types.Side.Sell,
				 Protobuf.Side.Buy => Core.Types.Side.Buy,
				 _ => throw new NotImplementedException()
			 };

		public static Core.Types.PositionSide ToPositionSideDto(this Protobuf.PositionSide sideInput)
			=> sideInput switch
			{
				Protobuf.PositionSide.Short => Core.Types.PositionSide.Short,
				Protobuf.PositionSide.Long => Core.Types.PositionSide.Long,
				Protobuf.PositionSide.Both => Core.Types.PositionSide.Both,
				_ => throw new NotImplementedException()
			};
	}
}
