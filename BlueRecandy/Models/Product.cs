using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueRecandy.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string? Description { get; set; }

		[Required]
		public bool UseExternalURL { get; set; }

		[Display(Name = "Download URL")]
		public string? DownloadURL { get; set; }

		public string SourceFileName { get; set; }
		public byte[]? SourceFileContents{ get; set; }
		public string SourceFileContentType { get; set; }


		[Required]
		[Range(0, int.MaxValue)]
		public double Price { get; set; }

		[Required]
		public string OwnerId { get; set; }
		[ForeignKey(nameof(OwnerId))]
		public ApplicationUser Owner { get; set; }

		public List<PurchaseLog> PurchaseLogs { get; set; }
	}
}
