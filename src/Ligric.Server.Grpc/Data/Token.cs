namespace Ligric.Server.Grpc.Data;

public class Token
{
    public string? JwtToken { get; }
    public DateTime Expiration { get; }

    public Token(string? jwtToken, DateTime expiration)
    {
        JwtToken = jwtToken;
        Expiration = expiration;
    }
}
