using BlueRecandy.Models;

namespace BlueRecandy.Services
{
	public interface IPurchaseLogsService
	{

		IEnumerable<PurchaseLog> GetPurchaseLogs();

		IEnumerable<PurchaseLog> GetPurchaseLogsByProductId(int productId);

		IEnumerable<PurchaseLog> GetPurchaseLogsByUserId(string userId);

		Task<int> AddPurchaseLog(PurchaseLog log);

	}
}
