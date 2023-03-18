namespace Ligric.Core.Ligric.Core.Types.Api
{
    public record ApiClientDto
    {
        public long? UserApiId { get; init; }

		public string Name { get; init; }

		/// <summary>
		/// Flag value: <see cref="ApiPermissions"/>
		/// </summary>
		public int Permissions { get; init; }

		public ApiClientDto(long? id, string name, int apiPermissions)
        {
            UserApiId = id;
            Name = name;
			Permissions = apiPermissions;
        }
    }
}
