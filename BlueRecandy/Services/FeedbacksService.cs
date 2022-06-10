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

        public async Task<Feedback?> GetFeedbacksById(int? id)
        {
            var feedback = await _context.Feedbacks
                .Include(f => f.Product)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            return feedback;
        }
    }
}
