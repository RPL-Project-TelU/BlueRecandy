using BlueRecandy.Models;
using System.Security.Claims;

namespace BlueRecandy.Services
{
	public interface IUsersService
	{

		Task<ApplicationUser> GetUserById(string id);

		Task<ApplicationUser> GetUserByEmail(string email);

		Task<ApplicationUser> GetUserByClaims(ClaimsPrincipal claim);

	}
}
