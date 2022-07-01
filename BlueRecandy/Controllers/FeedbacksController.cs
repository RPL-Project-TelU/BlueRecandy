#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlueRecandy.Data;
using BlueRecandy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BlueRecandy.Services;

namespace BlueRecandy.Controllers
{
    [Authorize] 
    public class FeedbacksController : Controller
    {
        private readonly IFeedbacksService _feedbackService;         
        private readonly IUsersService _usersService;

        public FeedbacksController(IFeedbacksService feedbackService, IUsersService usersService)
        {
            _feedbackService = feedbackService;
            _usersService = usersService;
        }

        // GET: Feedbacks
        public IActionResult Index()
        {
            return View(_feedbackService.GetAllFeedbacks());
        }

        // GET: Feedbacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _feedbackService.GetFeedbacksById(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Feedbacks/Create
        public IActionResult Create(int? productId)
        {
            ViewBag.ProductId = productId;
            TempData["ProductId"] = productId;
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FeedbackContent,Rating")] Feedback feedback)
        {
            var user = await _usersService.GetUserByClaims(User);

            var productId = TempData["ProductId"];

            feedback.UserId = user.Id;
            feedback.ProductId = (int) productId;

            await _feedbackService.AddFeedback(feedback);

            return RedirectToAction(nameof(Index));
        }

        // GET: Feedbacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _feedbackService.GetFeedbacksById(id);
            if (feedback == null)
            {
                return NotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FeedbackContent,Rating")] Feedback feedback)
        {
            if (id != feedback.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _feedbackService.UpdateFeedback(feedback);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackExists(feedback.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _feedbackService.GetFeedbacksById(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedback = await _feedbackService.GetFeedbacksById(id);
            await _feedbackService.DeleteFeedback(feedback);
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackExists(int id)
        {
            return _feedbackService.IsFeedbackExists(id);
        }
    }
}
