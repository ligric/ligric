namespace Ligric.Service.CryptoApisService.Domain
{
    public interface IBusinessRule
    {
        bool IsBroken();

        string Message { get; }
    }
}
