using BlueRecandy.Models;

namespace BlueRecandy.Services
{
    public interface IFeedbacksService
    {
        Task<Feedback?> GetFeedbacksById(int? id);
    }
}
