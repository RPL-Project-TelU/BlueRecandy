using BlueRecandy.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlueRecandy.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{

			builder.Entity<PurchaseLog>().HasKey(e => new
			{
				e.UserId,
				e.ProductId
			});

			builder.Entity<PurchaseLog>().HasOne(e => e.Product).WithMany(e => e.PurchaseLogs).HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.NoAction);
			builder.Entity<PurchaseLog>().HasOne(e => e.User).WithMany(e => e.PurchaseLogs).HasForeignKey(e => e.UserId);
			
			base.OnModelCreating(builder);
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<PurchaseLog> PurchaseLogs { get; set; }
	}
}