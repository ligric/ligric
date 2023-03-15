namespace Ligric.Service.CryptoApisService.Domain.Entities
{
	public class ApiEntity : EntityBase
	{
		public virtual string? PrivateKey { get; set; }

		public virtual string? PublicKey { get; set; }
	}
}
