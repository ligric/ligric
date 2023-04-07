namespace Ligric.Service.CryptoApisService.Domain.Model.Dtos.Response
{
	public record ApiClientResponseDto
	{
		public long UserApiId { get; init; }

		public string Name { get; init; }

		/// <summary>
		/// Flag value: <see cref="ApiPermissions"/>
		/// </summary>
		public int Permissions { get; init; }

		public ApiClientResponseDto(long id, string name, int apiPermissions)
		{
			UserApiId = id;
			Name = name;
			Permissions = apiPermissions;
		}
	}
}
