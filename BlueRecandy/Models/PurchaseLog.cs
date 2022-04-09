using System.ComponentModel.DataAnnotations;

namespace BlueRecandy.Models
{
	public class PurchaseLog
	{

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		public int ProductId { get; set; }
		public Product Product { get; set; }

	}
}
