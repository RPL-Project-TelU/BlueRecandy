using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlueRecandy.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required, Key]
		[MaxLength(36)]
		public override string Id { get; set; }

		public string? FullName { get; set; }

		public double Wallet { get; set; }

		public List<Product> OwnedProducts { get; set; }

		public List<PurchaseLog> PurchaseLogs { get; set; }
	}
}
