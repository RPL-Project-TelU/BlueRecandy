using BlueRecandy.Data;
using BlueRecandy.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlueRecandy.Services
{
	public class UsersService : IUsersService
	{

		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public UsersService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<ApplicationUser> GetUserByClaims(ClaimsPrincipal claim)
		{
			var result = await _userManager.GetUserAsync(claim);
			return result;
		}

		public async Task<ApplicationUser> GetUserByEmail(string email)
		{
			var found = await _userManager.FindByEmailAsync(email);
			return found;
		}

		public async Task<ApplicationUser> GetUserById(string id)
		{
			var found = await _userManager.FindByIdAsync(id);
			return found;
		}
	}
}
