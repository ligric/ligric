using System;

namespace Ligric.Service.AuthService.Domain.Entities
{
	public class EntityBase : EntityUnit
	{
		/// <summary>
		/// Gets or sets the create date.
		/// </summary>
		public virtual DateTime CreateDate { get; set; }

		/// <summary>
		/// Gets or sets the update date.
		/// </summary>
		public virtual DateTime? UpdateDate { get; set; }

		/// <summary>
		/// Gets or sets the deleted flag.
		/// </summary>
		public virtual bool Deleted { get; set; }

		/// <summary>
		/// If don't need to update Update Date - set it to True
		/// it's one time action!
		/// </summary>
		public virtual bool SkipUpdateDate { get; set; }
	}
}
