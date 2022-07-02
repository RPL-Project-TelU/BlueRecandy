using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BlueRecandy.Models
{
	[ExcludeFromCodeCoverage]
	public class PurchaseLog
	{

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		public int ProductId { get; set; }
		public Product Product { get; set; }

	}
}
