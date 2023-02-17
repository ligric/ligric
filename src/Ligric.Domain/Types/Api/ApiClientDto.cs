namespace Ligric.Domain.Types.Api
{
    public class ApiClientDto
    {
        public long? UserApiId { get; }

        public string Name { get; }

		/// <summary>
		/// Flag value: <see cref="ApiPermissions"/>
		/// </summary>
		public int Permissions { get; }

        public ApiClientDto(long? id, string name, int apiPermissions)
        {
            UserApiId = id;
            Name = name;
			Permissions = apiPermissions;
        }
    }
}
