namespace Ligric.UI.Infrastructure.Business.Data;

public partial record UserContext
{
    public string? Name { get; init; }

    public string? Email { get; init; }

    public string? AccessToken { get; init; }
}
