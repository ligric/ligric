namespace Ligric.Core.Types.Future
{
    public record FuturesOrderDto(long Id, string Symbol, Side Side, PositionSide PositionSide, decimal Quantity, decimal? Price, decimal? CurrentPrice, OrderType Type, decimal StopPrice);
}
