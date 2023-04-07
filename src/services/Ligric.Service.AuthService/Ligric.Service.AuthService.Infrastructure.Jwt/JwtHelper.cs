using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Ligric.Service.AuthService.Infrastructure.Jwt
{
	public class JwtHelper
	{
		public static IEnumerable<Claim> GetRolesAsClaims(IEnumerable<string> roles)
		{
			const string roleType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
			return roles.Select(x => new Claim(roleType, x));
		}
	}
}
