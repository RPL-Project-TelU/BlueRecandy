using System.ComponentModel.DataAnnotations;

namespace BlueRecandy.Models
{
	public class Feedback
	{

		[Key]
		public int Id { get; set; }

		[Required]
		public string FeedbackContent { get; set; }

		[Required]
		[Range(1, 5)]
		public int Rating { get; set; }

		[Required]
		public int ProductId { get; set; }
		public Product Product { get; set; }

		[Required]
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

	}
}
