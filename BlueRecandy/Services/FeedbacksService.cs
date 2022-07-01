using BlueRecandy.Data;
using BlueRecandy.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueRecandy.Services
{
    public class FeedbacksService : IFeedbacksService
    {
        private readonly ApplicationDbContext _context;

        public FeedbacksService(ApplicationDbContext context)
        {
            _context = context;
        }

		public async Task<int> AddFeedback(Feedback feedback)
		{
			_context.Add(feedback);
			return await _context.SaveChangesAsync();
		}

		public async Task<int> DeleteFeedback(Feedback feedback)
		{
			_context.Feedbacks.Remove(feedback);
			return await _context.SaveChangesAsync();
		}

		public IEnumerable<Feedback> GetAllFeedbacks()
		{
			return _context.Feedbacks.Include(f => f.User).Include(f => f.Product).AsEnumerable();
		}

		public async Task<Feedback?> GetFeedbacksById(int? id)
        {
            var feedback = await _context.Feedbacks
                .Include(f => f.Product)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            return feedback;
        }

		public bool IsFeedbackExists(int id)
		{
			return _context.Feedbacks.Any(x => x.Id == id);
		}

		public async Task<int> UpdateFeedback(Feedback feedback)
		{
			_context.Update(feedback);
			return await _context.SaveChangesAsync();
		}
	}
}
