using BlueRecandy.Models;

namespace BlueRecandy.Services
{
    public interface IFeedbacksService
    {
        Task<int> AddFeedback(Feedback feedback);

        Task<int> UpdateFeedback(Feedback feedback);

        Task<int> DeleteFeedback(Feedback feedback);

        IEnumerable<Feedback> GetAllFeedbacks();

        Task<Feedback?> GetFeedbacksById(int? id);

        bool IsFeedbackExists(int id);
    }
}
