using Ligric.Protos;
using Ligric.Server.IntegrationTests;

namespace Ligric.Server.Tests
{
	public class AuthorizationServiceTests : PermissionsServerTestBase<Authorization.AuthorizationClient>
	{

		public AuthorizationServiceTests(PermissionsServerApplicationFactory factory) : base(factory)
		{
		}

		[Fact]
		public void IsUserAllowed_ForCreateTodoPermissions_ShouldReturnFalse()
		{
			// Arrange
			var request = new SignInRequest() { Login = "test", Password = "test" };
			// Act
			var response = this.Client?.SignIn(request);
			// Assert
			Assert.True(response?.Result.IsSuccess);
			Assert.NotNull(response?.JwtToken?.Token);
		}
	}
}
