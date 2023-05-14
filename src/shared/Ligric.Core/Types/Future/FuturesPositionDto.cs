namespace Ligric.Core.Types.Future
{
	public record FuturesPositionDto(long Id, string Symbol, Side Side, decimal Quantity, decimal EntryPrice, byte? Leverage);
}
