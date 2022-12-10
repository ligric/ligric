namespace Ligric.Common.Types
{
    public class ApiClientDto
    {
        public long? Id { get; }

        public string Name { get; }

        public ApiClientDto(long? id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
