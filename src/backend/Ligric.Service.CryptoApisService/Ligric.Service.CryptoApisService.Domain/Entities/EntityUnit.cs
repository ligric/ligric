namespace Ligric.Service.CryptoApisService.Domain.Entities
{
	public class EntityUnit
	{
		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		public virtual long? Id { get; set; }

		/// <summary>
		/// Gets a value indicating whether is new.
		/// </summary>
		public virtual bool IsNew => !Id.HasValue;
	}
}
