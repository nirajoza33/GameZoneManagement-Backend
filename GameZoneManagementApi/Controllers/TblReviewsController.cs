//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using GameZoneManagementApi.Models;
//using GameZoneManagementApi.DTOs;

//namespace GameZoneManagementApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TblReviewsController : ControllerBase
//    {
//        private readonly GamezoneDbContext _context;

//        public TblReviewsController(GamezoneDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/TblReviews
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<TblReview>>> GetTblReviews()
//        {
//            return await _context.TblReviews.ToListAsync();
//        }

//        // GET: api/TblReviews/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<TblReview>> GetTblReview(int id)
//        {
//            var tblReview = await _context.TblReviews.FindAsync(id);

//            if (tblReview == null)
//            {
//                return NotFound();
//            }

//            return tblReview;
//        }

//        // PUT: api/TblReviews/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutTblReview(int id, TblReview tblReview)
//        {
//            if (id != tblReview.ReviewId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(tblReview).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!TblReviewExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/TblReviews
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<TblReview>> PostTblReview(ReviewDto reviewDto)
//        {

//            var review = new TblReview
//            {
//                GameId = reviewDto.GameId,
//                UserId = reviewDto.UserId,
//                Rating = reviewDto.Rating,
//                ReviewText = reviewDto.ReviewText
//            };

//            _context.TblReviews.Add(review);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetTblReview", new { id = review.ReviewId }, review);
//        }

//        // DELETE: api/TblReviews/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteTblReview(int id)
//        {
//            var tblReview = await _context.TblReviews.FindAsync(id);
//            if (tblReview == null)
//            {
//                return NotFound();
//            }

//            _context.TblReviews.Remove(tblReview);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool TblReviewExists(int id)
//        {
//            return _context.TblReviews.Any(e => e.ReviewId == id);
//        }
//    }
//}


// --------------------------------------------------------------------------------------------





//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using GameZoneManagementApi.Models;
//using GameZoneManagementApi.DTOs;

//namespace GameZoneManagementApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TblReviewsController : ControllerBase
//    {
//        private readonly GamezoneDbContext _context;

//        public TblReviewsController(GamezoneDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/TblReviews
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetTblReviews()
//        {
//            var reviews = await _context.TblReviews
//                .Include(r => r.Game)
//                .Include(r => r.User)
//                .ToListAsync();

//            var reviewDtos = reviews.Select(r => new ReviewResponseDto
//            {
//                ReviewId = r.ReviewId,
//                GameId = r.GameId,
//                GameTitle = r.Game.Title,
//                UserId = r.UserId,
//                UserName = r.User.UserName,
//                UserImage = "/placeholder.svg?height=80&width=80",
//                Rating = r.Rating,
//                Title = r.Title,
//                ReviewText = r.ReviewText,
//                CreatedDate = r.CreatedDate,
//                Likes = r.Likes,
//                Replies = r.Replies,
//                Sentiment = DetermineSentiment(r.Rating),
//                Verified = true
//            }).ToList();

//            return reviewDtos;
//        }

//        // GET: api/TblReviews/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<ReviewResponseDto>> GetTblReview(int id)
//        {
//            var review = await _context.TblReviews
//                .Include(r => r.Game)
//                .Include(r => r.User)
//                .FirstOrDefaultAsync(r => r.ReviewId == id);

//            if (review == null)
//            {
//                return NotFound();
//            }

//            var reviewDto = new ReviewResponseDto
//            {
//                ReviewId = review.ReviewId,
//                GameId = review.GameId,
//                GameTitle = review.Game.Title,
//                UserId = review.UserId,
//                UserName = review.User.UserName,
//                UserImage = "/placeholder.svg?height=80&width=80",
//                Rating = review.Rating,
//                Title = review.Title,
//                ReviewText = review.ReviewText,
//                CreatedDate = review.CreatedDate,
//                Likes = review.Likes,
//                Replies = review.Replies,
//                Sentiment = DetermineSentiment(review.Rating),
//                Verified = true
//            };

//            return reviewDto;
//        }

//        // GET: api/TblReviews/Game/5
//        [HttpGet("Game/{gameId}")]
//        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetReviewsByGame(int gameId)
//        {
//            var reviews = await _context.TblReviews
//                .Include(r => r.Game)
//                .Include(r => r.User)
//                .Where(r => r.GameId == gameId)
//                .ToListAsync();

//            var reviewDtos = reviews.Select(r => new ReviewResponseDto
//            {
//                ReviewId = r.ReviewId,
//                GameId = r.GameId,
//                GameTitle = r.Game.Title,
//                UserId = r.UserId,
//                UserName = r.User.UserName,
//                UserImage = "/placeholder.svg?height=80&width=80",
//                Rating = r.Rating,
//                Title = r.Title,
//                ReviewText = r.ReviewText,
//                CreatedDate = r.CreatedDate,
//                Likes = r.Likes,
//                Replies = r.Replies,
//                Sentiment = DetermineSentiment(r.Rating),
//                Verified = true
//            }).ToList();

//            return reviewDtos;
//        }

//        // GET: api/TblReviews/User/5
//        [HttpGet("User/{userId}")]
//        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetReviewsByUser(int userId)
//        {
//            var reviews = await _context.TblReviews
//                .Include(r => r.Game)
//                .Include(r => r.User)
//                .Where(r => r.UserId == userId)
//                .ToListAsync();

//            var reviewDtos = reviews.Select(r => new ReviewResponseDto
//            {
//                ReviewId = r.ReviewId,
//                GameId = r.GameId,
//                GameTitle = r.Game.Title,
//                UserId = r.UserId,
//                UserName = r.User.UserName,
//                UserImage = "/placeholder.svg?height=80&width=80",
//                Rating = r.Rating,
//                Title = r.Title,
//                ReviewText = r.ReviewText,
//                CreatedDate = r.CreatedDate,
//                Likes = r.Likes,
//                Replies = r.Replies,
//                Sentiment = DetermineSentiment(r.Rating),
//                Verified = true
//            }).ToList();

//            return reviewDtos;
//        }

//        // POST: api/TblReviews/5/like
//        [HttpPost("{reviewId}/like")]
//        public async Task<ActionResult> LikeReview(int reviewId, [FromBody] ReviewLikeDto likeDto)
//        {
//            var review = await _context.TblReviews.FindAsync(reviewId);
//            if (review == null)
//            {
//                return NotFound("Review not found");
//            }

//            // Check if user already liked this review
//            var existingLike = await _context.TblReviewLikes
//                .FirstOrDefaultAsync(rl => rl.ReviewId == reviewId && rl.UserId == likeDto.UserId);

//            if (existingLike != null)
//            {
//                return BadRequest("User has already liked this review");
//            }

//            // Add like record
//            var reviewLike = new TblReviewLike
//            {
//                ReviewId = reviewId,
//                UserId = likeDto.UserId,
//                CreatedDate = DateTime.Now
//            };

//            _context.TblReviewLikes.Add(reviewLike);

//            // Update likes count
//            review.Likes += 1;

//            await _context.SaveChangesAsync();

//            return Ok(new { 
//                success = true, 
//                likes = review.Likes,
//                message = "Review liked successfully" 
//            });
//        }

//        // DELETE: api/TblReviews/5/like
//        [HttpDelete("{reviewId}/like")]
//        public async Task<ActionResult> UnlikeReview(int reviewId, [FromBody] ReviewLikeDto likeDto)
//        {
//            var review = await _context.TblReviews.FindAsync(reviewId);
//            if (review == null)
//            {
//                return NotFound("Review not found");
//            }

//            // Find existing like
//            var existingLike = await _context.TblReviewLikes
//                .FirstOrDefaultAsync(rl => rl.ReviewId == reviewId && rl.UserId == likeDto.UserId);

//            if (existingLike == null)
//            {
//                return BadRequest("User has not liked this review");
//            }

//            // Remove like record
//            _context.TblReviewLikes.Remove(existingLike);

//            // Update likes count
//            review.Likes = Math.Max(0, review.Likes - 1);

//            await _context.SaveChangesAsync();

//            return Ok(new { 
//                success = true, 
//                likes = review.Likes,
//                message = "Review unliked successfully" 
//            });
//        }

//        // GET: api/TblReviews/5/likes/check/123
//        [HttpGet("{reviewId}/likes/check/{userId}")]
//        public async Task<ActionResult> CheckUserLike(int reviewId, int userId)
//        {
//            var hasLiked = await _context.TblReviewLikes
//                .AnyAsync(rl => rl.ReviewId == reviewId && rl.UserId == userId);

//            return Ok(new { hasLiked });
//        }

//        // POST: api/TblReviews/5/reply
//        [HttpPost("{reviewId}/reply")]
//        public async Task<ActionResult<ReviewReplyResponseDto>> AddReply(int reviewId, [FromBody] ReviewReplyDto replyDto)
//        {
//            var review = await _context.TblReviews.FindAsync(reviewId);
//            if (review == null)
//            {
//                return NotFound("Review not found");
//            }

//            var user = await _context.Tblusers.FindAsync(replyDto.UserId);
//            if (user == null)
//            {
//                return NotFound("User not found");
//            }

//            var reply = new TblReviewReply
//            {
//                ReviewId = reviewId,
//                UserId = replyDto.UserId,
//                ReplyText = replyDto.ReplyText,
//                CreatedDate = DateTime.Now
//            };

//            _context.TblReviewReplies.Add(reply);

//            // Update replies count
//            review.Replies += 1;

//            await _context.SaveChangesAsync();

//            // Return the created reply with user info
//            var replyResponse = new ReviewReplyResponseDto
//            {
//                ReplyId = reply.ReplyId,
//                ReviewId = reply.ReviewId,
//                UserId = reply.UserId,
//                UserName = user.UserName,
//                UserImage = "/placeholder.svg?height=32&width=32",
//                ReplyText = reply.ReplyText,
//                CreatedDate = reply.CreatedDate
//            };

//            return CreatedAtAction(nameof(GetReply), new { replyId = reply.ReplyId }, replyResponse);
//        }

//        // GET: api/TblReviews/5/replies
//        [HttpGet("{reviewId}/replies")]
//        public async Task<ActionResult<IEnumerable<ReviewReplyResponseDto>>> GetReplies(int reviewId)
//        {
//            var replies = await _context.TblReviewReplies
//                .Include(rr => rr.User)
//                .Where(rr => rr.ReviewId == reviewId)
//                .OrderBy(rr => rr.CreatedDate)
//                .ToListAsync();

//            var replyDtos = replies.Select(r => new ReviewReplyResponseDto
//            {
//                ReplyId = r.ReplyId,
//                ReviewId = r.ReviewId,
//                UserId = r.UserId,
//                UserName = r.User.UserName,
//                UserImage = "/placeholder.svg?height=32&width=32",
//                ReplyText = r.ReplyText,
//                CreatedDate = r.CreatedDate
//            }).ToList();

//            return Ok(replyDtos);
//        }

//        // GET: api/TblReviews/replies/5
//        [HttpGet("replies/{replyId}")]
//        public async Task<ActionResult<ReviewReplyResponseDto>> GetReply(int replyId)
//        {
//            var reply = await _context.TblReviewReplies
//                .Include(rr => rr.User)
//                .FirstOrDefaultAsync(rr => rr.ReplyId == replyId);

//            if (reply == null)
//            {
//                return NotFound();
//            }

//            var replyDto = new ReviewReplyResponseDto
//            {
//                ReplyId = reply.ReplyId,
//                ReviewId = reply.ReviewId,
//                UserId = reply.UserId,
//                UserName = reply.User.UserName,
//                UserImage = "/placeholder.svg?height=32&width=32",
//                ReplyText = reply.ReplyText,
//                CreatedDate = reply.CreatedDate
//            };

//            return Ok(replyDto);
//        }

//        // DELETE: api/TblReviews/replies/5
//        [HttpDelete("replies/{replyId}")]
//        public async Task<ActionResult> DeleteReply(int replyId, [FromBody] DeleteReplyDto deleteDto)
//        {
//            var reply = await _context.TblReviewReplies.FindAsync(replyId);
//            if (reply == null)
//            {
//                return NotFound("Reply not found");
//            }

//            // Only allow the reply author to delete their reply
//            if (reply.UserId != deleteDto.UserId)
//            {
//                return Forbid("You can only delete your own replies");
//            }

//            var review = await _context.TblReviews.FindAsync(reply.ReviewId);
//            if (review != null)
//            {
//                review.Replies = Math.Max(0, review.Replies - 1);
//            }

//            _context.TblReviewReplies.Remove(reply);
//            await _context.SaveChangesAsync();

//            return Ok(new { message = "Reply deleted successfully" });
//        }

//        // PUT: api/TblReviews/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutTblReview(int id, ReviewDto reviewDto)
//        {
//            var review = await _context.TblReviews.FindAsync(id);

//            if (review == null)
//            {
//                return NotFound();
//            }

//            // Only update if the user is the owner of the review
//            if (review.UserId != reviewDto.UserId)
//            {
//                return Forbid();
//            }

//            review.GameId = reviewDto.GameId;
//            review.Rating = reviewDto.Rating;
//            review.Title = reviewDto.Title;
//            review.ReviewText = reviewDto.ReviewText;
//            review.Sentiment = DetermineSentiment(reviewDto.Rating);

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!TblReviewExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/TblReviews
//        [HttpPost]
//        public async Task<ActionResult<ReviewResponseDto>> PostTblReview(ReviewDto reviewDto)
//        {
//            var review = new TblReview
//            {
//                GameId = reviewDto.GameId,
//                UserId = reviewDto.UserId,
//                Rating = reviewDto.Rating,
//                Title = reviewDto.Title ?? "Review",
//                ReviewText = reviewDto.ReviewText,
//                CreatedDate = DateTime.Now,
//                Likes = 0,
//                Replies = 0,
//                Sentiment = DetermineSentiment(reviewDto.Rating)
//            };

//            _context.TblReviews.Add(review);
//            await _context.SaveChangesAsync();

//            // Get the full review with related entities
//            var createdReview = await _context.TblReviews
//                .Include(r => r.Game)
//                .Include(r => r.User)
//                .FirstOrDefaultAsync(r => r.ReviewId == review.ReviewId);

//            var reviewResponseDto = new ReviewResponseDto
//            {
//                ReviewId = createdReview.ReviewId,
//                GameId = createdReview.GameId,
//                GameTitle = createdReview.Game.Title,
//                UserId = createdReview.UserId,
//                UserName = createdReview.User.UserName,
//                UserImage = "/placeholder.svg?height=80&width=80",
//                Rating = createdReview.Rating,
//                Title = createdReview.Title,
//                ReviewText = createdReview.ReviewText,
//                CreatedDate = createdReview.CreatedDate,
//                Likes = createdReview.Likes,
//                Replies = createdReview.Replies,
//                Sentiment = DetermineSentiment(createdReview.Rating),
//                Verified = true
//            };

//            return CreatedAtAction("GetTblReview", new { id = review.ReviewId }, reviewResponseDto);
//        }

//        // DELETE: api/TblReviews/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteTblReview(int id)
//        {
//            var tblReview = await _context.TblReviews.FindAsync(id);
//            if (tblReview == null)
//            {
//                return NotFound();
//            }

//            _context.TblReviews.Remove(tblReview);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool TblReviewExists(int id)
//        {
//            return _context.TblReviews.Any(e => e.ReviewId == id);
//        }

//        // Make this method static to avoid the EF Core error
//        private static string DetermineSentiment(int rating)
//        {
//            if (rating >= 4) return "positive";
//            if (rating == 3) return "neutral";
//            return "negative";
//        }
//    }
//}

//// DTOs for the new functionality
//public class ReviewLikeDto
//{
//    public int UserId { get; set; }
//}

//public class ReviewReplyDto
//{
//    public int UserId { get; set; }
//    public string ReplyText { get; set; }
//}

//public class ReviewReplyResponseDto
//{
//    public int ReplyId { get; set; }
//    public int ReviewId { get; set; }
//    public int UserId { get; set; }
//    public string UserName { get; set; }
//    public string UserImage { get; set; }
//    public string ReplyText { get; set; }
//    public DateTime CreatedDate { get; set; }
//}

//public class DeleteReplyDto
//{
//    public int UserId { get; set; }
//}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameZoneManagementApi.Models;
using GameZoneManagementApi.DTOs;
using System.Text.Json;

namespace GameZoneManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblReviewsController : ControllerBase
    {
        private readonly GamezoneDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TblReviewsController(GamezoneDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/TblReviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetTblReviews()
        {
            var reviews = await _context.TblReviews
                .Include(r => r.Game)
                .Include(r => r.User)
                .Where(r => !r.IsDeleted)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            var reviewDtos = reviews.Select(r => new ReviewResponseDto
            {
                ReviewId = r.ReviewId,
                GameId = r.GameId,
                GameTitle = r.Game?.Title ?? "Unknown Game",
                UserId = r.UserId,
                UserName = r.User?.UserName ?? "Unknown User",
                Rating = r.Rating,
                Title = r.Title,
                ReviewText = r.ReviewText,
                CreatedDate = r.CreatedDate,
                Likes = r.Likes,
                Replies = r.Replies,
                Sentiment = DetermineSentiment(r.Rating),
                Verified = true,
                ImageUrls = ParseImageUrls(r.ImageUrls)
            }).ToList();

            return reviewDtos;
        }

        // GET: api/TblReviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewResponseDto>> GetTblReview(int id)
        {
            var review = await _context.TblReviews
                .Include(r => r.Game)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReviewId == id && !r.IsDeleted);

            if (review == null)
            {
                return NotFound();
            }

            var reviewDto = new ReviewResponseDto
            {
                ReviewId = review.ReviewId,
                GameId = review.GameId,
                GameTitle = review.Game?.Title ?? "Unknown Game",
                UserId = review.UserId,
                UserName = review.User?.UserName ?? "Unknown User",
                Rating = review.Rating,
                Title = review.Title,
                ReviewText = review.ReviewText,
                CreatedDate = review.CreatedDate,
                Likes = review.Likes,
                Replies = review.Replies,
                Sentiment = DetermineSentiment(review.Rating),
                Verified = true,
                ImageUrls = ParseImageUrls(review.ImageUrls)
            };

            return reviewDto;
        }

        // POST: api/TblReviews - Create new review with enhanced error handling
        [HttpPost]
        public async Task<ActionResult<ReviewResponseDto>> PostTblReview([FromBody] ReviewDto reviewDto)
        {
            try
            {
                // Validate input data
                if (reviewDto == null)
                {
                    return BadRequest(new { message = "Review data is required" });
                }

                if (reviewDto.GameId <= 0)
                {
                    return BadRequest(new { message = "Valid GameId is required" });
                }

                if (reviewDto.UserId <= 0)
                {
                    return BadRequest(new { message = "Valid UserId is required" });
                }

                if (reviewDto.Rating < 1 || reviewDto.Rating > 5)
                {
                    return BadRequest(new { message = "Rating must be between 1 and 5" });
                }

                if (string.IsNullOrWhiteSpace(reviewDto.ReviewText))
                {
                    return BadRequest(new { message = "Review text is required" });
                }

                // Check if game exists
                var gameExists = await _context.Games.AnyAsync(g => g.GameId == reviewDto.GameId);
                if (!gameExists)
                {
                    return BadRequest(new { message = $"Game with ID {reviewDto.GameId} does not exist" });
                }

                // Check if user exists
                var userExists = await _context.Tblusers.AnyAsync(u => u.UserId == reviewDto.UserId);
                if (!userExists)
                {
                    return BadRequest(new { message = $"User with ID {reviewDto.UserId} does not exist" });
                }

                // Check if user already reviewed this game
                var existingReview = await _context.TblReviews
                    .FirstOrDefaultAsync(r => r.GameId == reviewDto.GameId && r.UserId == reviewDto.UserId && !r.IsDeleted);

                if (existingReview != null)
                {
                    return BadRequest(new { message = "You have already reviewed this game" });
                }

                var review = new TblReview
                {
                    GameId = reviewDto.GameId,
                    UserId = reviewDto.UserId,
                    Rating = reviewDto.Rating,
                    Title = !string.IsNullOrWhiteSpace(reviewDto.Title) ? reviewDto.Title.Trim() : "Review",
                    ReviewText = reviewDto.ReviewText.Trim(),
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = null,
                    Likes = 0,
                    Replies = 0,
                    Sentiment = DetermineSentiment(reviewDto.Rating),
                    IsDeleted = false,
                    ImageUrls = reviewDto.ImageUrls?.Count > 0 ? JsonSerializer.Serialize(reviewDto.ImageUrls) : null
                };

                _context.TblReviews.Add(review);
                await _context.SaveChangesAsync();

                // Get the full review with related entities
                var createdReview = await _context.TblReviews
                    .Include(r => r.Game)
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.ReviewId == review.ReviewId);

                if (createdReview == null)
                {
                    return StatusCode(500, new { message = "Review was created but could not be retrieved" });
                }

                var reviewResponseDto = new ReviewResponseDto
                {
                    ReviewId = createdReview.ReviewId,
                    GameId = createdReview.GameId,
                    GameTitle = createdReview.Game?.Title ?? "Unknown Game",
                    UserId = createdReview.UserId,
                    UserName = createdReview.User?.UserName ?? "Unknown User",
                    Rating = createdReview.Rating,
                    Title = createdReview.Title,
                    ReviewText = createdReview.ReviewText,
                    CreatedDate = createdReview.CreatedDate,
                    Likes = createdReview.Likes,
                    Replies = createdReview.Replies,
                    Sentiment = createdReview.Sentiment,
                    Verified = true,
                    ImageUrls = reviewDto.ImageUrls ?? new List<string>()
                };

                return CreatedAtAction("GetTblReview", new { id = review.ReviewId }, reviewResponseDto);
            }
            catch (DbUpdateException dbEx)
            {
                // Log the detailed database error
                var innerException = dbEx.InnerException?.Message ?? dbEx.Message;
                return BadRequest(new
                {
                    message = "Database error occurred while creating review",
                    error = innerException,
                    details = "Please check if the GameId and UserId are valid and exist in the database"
                });
            }
            catch (Exception ex)
            {
                // Log the general error
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred while creating review",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        // POST: api/TblReviews/upload - Upload file endpoint with specific route
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No file uploaded" });
                }

                // Validate file size (5MB max)
                if (file.Length > 5 * 1024 * 1024)
                {
                    return BadRequest(new { message = "File size must be less than 5MB" });
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { message = "Only image files are allowed" });
                }

                // Create uploads directory if it doesn't exist
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new
                {
                    fileName = fileName,
                    url = $"/uploads/{fileName}",
                    message = "File uploaded successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error uploading file", error = ex.Message });
            }
        }

        // POST: api/TblReviews/5/like
        [HttpPost("{reviewId}/like")]
        public async Task<ActionResult> LikeReview(int reviewId, [FromBody] ReviewLikeDto likeDto)
        {
            var review = await _context.TblReviews.FindAsync(reviewId);
            if (review == null)
            {
                return NotFound("Review not found");
            }

            // Check if user already liked this review
            var existingLike = await _context.TblReviewLikes
                .FirstOrDefaultAsync(rl => rl.ReviewId == reviewId && rl.UserId == likeDto.UserId);

            if (existingLike != null)
            {
                return BadRequest(new { message = "You have already liked this review" });
            }

            // Like the review
            var reviewLike = new TblReviewLike
            {
                ReviewId = reviewId,
                UserId = likeDto.UserId,
                CreatedDate = DateTime.UtcNow
            };

            _context.TblReviewLikes.Add(reviewLike);
            review.Likes += 1;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                liked = true,
                likes = review.Likes,
                message = "Review liked successfully"
            });
        }

        // DELETE: api/TblReviews/5/like
        [HttpDelete("{reviewId}/like")]
        public async Task<ActionResult> UnlikeReview(int reviewId, [FromBody] ReviewLikeDto likeDto)
        {
            var review = await _context.TblReviews.FindAsync(reviewId);
            if (review == null)
            {
                return NotFound("Review not found");
            }

            var existingLike = await _context.TblReviewLikes
                .FirstOrDefaultAsync(rl => rl.ReviewId == reviewId && rl.UserId == likeDto.UserId);

            if (existingLike == null)
            {
                return BadRequest(new { message = "You haven't liked this review" });
            }

            _context.TblReviewLikes.Remove(existingLike);
            review.Likes = Math.Max(0, review.Likes - 1);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                liked = false,
                likes = review.Likes,
                message = "Review unliked successfully"
            });
        }

        // GET: api/TblReviews/5/likes/check/123
        [HttpGet("{reviewId}/likes/check/{userId}")]
        public async Task<ActionResult> CheckUserLike(int reviewId, int userId)
        {
            var hasLiked = await _context.TblReviewLikes
                .AnyAsync(rl => rl.ReviewId == reviewId && rl.UserId == userId);

            return Ok(new { hasLiked });
        }

        // POST: api/TblReviews/5/reply
        [HttpPost("{reviewId}/reply")]
        public async Task<ActionResult<ReviewReplyResponseDto>> AddReply(int reviewId, [FromBody] ReviewReplyDto replyDto)
        {
            var review = await _context.TblReviews.FindAsync(reviewId);
            if (review == null)
            {
                return NotFound("Review not found");
            }

            var user = await _context.Tblusers.FindAsync(replyDto.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var reply = new TblReviewReply
            {
                ReviewId = reviewId,
                UserId = replyDto.UserId,
                ReplyText = replyDto.ReplyText,
                CreatedDate = DateTime.UtcNow
            };

            _context.TblReviewReplies.Add(reply);
            review.Replies += 1;
            await _context.SaveChangesAsync();

            var replyResponse = new ReviewReplyResponseDto
            {
                ReplyId = reply.ReplyId,
                ReviewId = reply.ReviewId,
                UserId = reply.UserId,
                UserName = user.UserName,
                ReplyText = reply.ReplyText,
                CreatedDate = reply.CreatedDate
            };

            return CreatedAtAction(nameof(GetReply), new { replyId = reply.ReplyId }, replyResponse);
        }

        // GET: api/TblReviews/5/replies
        [HttpGet("{reviewId}/replies")]
        public async Task<ActionResult<IEnumerable<ReviewReplyResponseDto>>> GetReplies(int reviewId)
        {
            var replies = await _context.TblReviewReplies
                .Include(rr => rr.User)
                .Where(rr => rr.ReviewId == reviewId && !rr.IsDeleted)
                .OrderBy(rr => rr.CreatedDate)
                .ToListAsync();

            var replyDtos = replies.Select(r => new ReviewReplyResponseDto
            {
                ReplyId = r.ReplyId,
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                UserName = r.User?.UserName ?? "Unknown User",
                ReplyText = r.ReplyText,
                CreatedDate = r.CreatedDate
            }).ToList();

            return Ok(replyDtos);
        }

        // GET: api/TblReviews/replies/5
        [HttpGet("replies/{replyId}")]
        public async Task<ActionResult<ReviewReplyResponseDto>> GetReply(int replyId)
        {
            var reply = await _context.TblReviewReplies
                .Include(rr => rr.User)
                .FirstOrDefaultAsync(rr => rr.ReplyId == replyId && !rr.IsDeleted);

            if (reply == null)
            {
                return NotFound();
            }

            var replyDto = new ReviewReplyResponseDto
            {
                ReplyId = reply.ReplyId,
                ReviewId = reply.ReviewId,
                UserId = reply.UserId,
                UserName = reply.User?.UserName ?? "Unknown User",
                ReplyText = reply.ReplyText,
                CreatedDate = reply.CreatedDate
            };

            return Ok(replyDto);
        }

        // DELETE: api/TblReviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblReview(int id)
        {
            var tblReview = await _context.TblReviews.FindAsync(id);
            if (tblReview == null)
            {
                return NotFound();
            }

            // Soft delete
            tblReview.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper Methods
        private bool TblReviewExists(int id)
        {
            return _context.TblReviews.Any(e => e.ReviewId == id && !e.IsDeleted);
        }

        private static string DetermineSentiment(int rating)
        {
            if (rating >= 4) return "positive";
            if (rating == 3) return "neutral";
            return "negative";
        }

        private List<string> ParseImageUrls(string imageUrlsJson)
        {
            if (string.IsNullOrEmpty(imageUrlsJson))
                return new List<string>();

            try
            {
                return JsonSerializer.Deserialize<List<string>>(imageUrlsJson) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }


        // GET: api/TblReviews/game/5 - Get reviews for a specific game
        [HttpGet("game/{gameId}")]
        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetReviewsByGameId(int gameId)
        {
            var reviews = await _context.TblReviews
                .Include(r => r.Game)
                .Include(r => r.User)
                .Where(r => r.GameId == gameId && !r.IsDeleted)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            var reviewDtos = reviews.Select(r => new ReviewResponseDto
            {
                ReviewId = r.ReviewId,
                GameId = r.GameId,
                GameTitle = r.Game?.Title ?? "Unknown Game",
                UserId = r.UserId,
                UserName = r.User?.UserName ?? "Unknown User",
                Rating = r.Rating,
                Title = r.Title,
                ReviewText = r.ReviewText,
                CreatedDate = r.CreatedDate,
                Likes = r.Likes,
                Replies = r.Replies,
                Sentiment = DetermineSentiment(r.Rating),
                Verified = true,
                ImageUrls = ParseImageUrls(r.ImageUrls)
            }).ToList();

            return reviewDtos;
        }
    }

    // DTO for reply submission
    public class ReviewReplyDto
    {
        public int UserId { get; set; }
        public string ReplyText { get; set; }
    }
}
