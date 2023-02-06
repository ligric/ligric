namespace Ligric.Server.Grpc.Data;

public class UserResponse
{
    public Guid Id { get; }
    public string Login { get; }
    public string Email { get; }
    public Token? Token { get; }

    public UserResponse(Guid id, string login, string email, Token? token)
    {
        Id = id;
        Login = login;
        Email = email;
        Token = token;
    }
}
