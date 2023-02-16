namespace Ligric.Domain.Types.Api
{
    public class ApiClientDto
    {
        public long? Id { get; }

        public string Name { get; }

		public ApiPermissions Permissions { get; }

        public ApiClientDto(long? id, string name, ApiPermissions apiPermissions)
        {
            Id = id;
            Name = name;
			Permissions = apiPermissions;
        }
    }
}
