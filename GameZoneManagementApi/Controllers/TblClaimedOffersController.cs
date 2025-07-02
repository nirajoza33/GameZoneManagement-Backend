//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using GameZoneManagementApi.Models;

//namespace GameZoneManagementApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TblClaimedOffersController : ControllerBase
//    {
//        private readonly GamezoneDbContext _context;

//        public TblClaimedOffersController(GamezoneDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/TblClaimedOffers
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<TblClaimedOffer>>> GetTblClaimedOffer()
//        {
//            return await _context.TblClaimedOffer.ToListAsync();
//        }

//        // GET: api/TblClaimedOffers/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<TblClaimedOffer>> GetTblClaimedOffer(int id)
//        {
//            var tblClaimedOffer = await _context.TblClaimedOffer.FindAsync(id);

//            if (tblClaimedOffer == null)
//            {
//                return NotFound();
//            }

//            return tblClaimedOffer;
//        }

//        // PUT: api/TblClaimedOffers/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutTblClaimedOffer(int id, TblClaimedOffer tblClaimedOffer)
//        {
//            if (id != tblClaimedOffer.ClaimedOfferId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(tblClaimedOffer).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!TblClaimedOfferExists(id))
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

//        // POST: api/TblClaimedOffers
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<TblClaimedOffer>> PostTblClaimedOffer(TblClaimedOffer tblClaimedOffer)
//        {
//            _context.TblClaimedOffer.Add(tblClaimedOffer);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetTblClaimedOffer", new { id = tblClaimedOffer.ClaimedOfferId }, tblClaimedOffer);
//        }

//        // DELETE: api/TblClaimedOffers/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteTblClaimedOffer(int id)
//        {
//            var tblClaimedOffer = await _context.TblClaimedOffer.FindAsync(id);
//            if (tblClaimedOffer == null)
//            {
//                return NotFound();
//            }

//            _context.TblClaimedOffer.Remove(tblClaimedOffer);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool TblClaimedOfferExists(int id)
//        {
//            return _context.TblClaimedOffer.Any(e => e.ClaimedOfferId == id);
//        }
//    }
//}




using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameZoneManagementApi.Models;
using GameZoneManagementApi.DTOs;
using System.Text.Json;

namespace GameZoneManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblClaimedOffersController : ControllerBase
    {
        private readonly GamezoneDbContext _context;

        public TblClaimedOffersController(GamezoneDbContext context)
        {
            _context = context;
        }

        // GET: api/TblClaimedOffers/ByUser/5
        [HttpGet("ByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<ClaimedOfferDto>>> GetClaimedOffersByUser(int userId)
        {
            try
            {
                var claimedOffers = await _context.TblClaimedOffer
                    .Include(co => co.Offer)
                    .Where(co => co.UserId == userId && co.IsActive)
                    .OrderByDescending(co => co.ClaimedDate)
                    .ToListAsync();

                var result = claimedOffers.Select(co => MapToClaimedOfferDto(co)).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // GET: api/TblClaimedOffers/Active/5
        [HttpGet("Active/{userId}")]
        public async Task<ActionResult<IEnumerable<ClaimedOfferDto>>> GetActiveClaimedOffersByUser(int userId)
        {
            try
            {
                var claimedOffers = await _context.TblClaimedOffer
                    .Include(co => co.Offer)
                    .Where(co => co.UserId == userId && co.IsActive && !co.IsUsed && co.Offer.ValidUntil >= DateTime.Now)
                    .OrderByDescending(co => co.ClaimedDate)
                    .ToListAsync();

                var result = claimedOffers.Select(co => MapToClaimedOfferDto(co)).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // POST: api/TblClaimedOffers/Claim
        [HttpPost("Claim")]
        public async Task<ActionResult<ClaimedOfferDto>> ClaimOffer([FromBody] ClaimOfferRequest request)
        {
            try
            {
                // Validate request
                if (request.UserId <= 0 || request.OfferId <= 0)
                {
                    return BadRequest(new { success = false, message = "Invalid user ID or offer ID" });
                }

                // Check if user exists
                var userExists = await _context.Tblusers.AnyAsync(u => u.UserId == request.UserId);
                if (!userExists)
                {
                    return BadRequest(new { success = false, message = "User not found" });
                }

                // Check if offer exists and is valid
                var offer = await _context.TblOffers.FindAsync(request.OfferId);
                if (offer == null)
                {
                    return NotFound(new { success = false, message = "Offer not found" });
                }

                if (!offer.Status || offer.ValidUntil < DateTime.Now)
                {
                    return BadRequest(new { success = false, message = "This offer is no longer valid" });
                }

                // Check if user has already claimed this offer
                var existingClaim = await _context.TblClaimedOffer
                    .FirstOrDefaultAsync(co => co.UserId == request.UserId && co.OfferId == request.OfferId && co.IsActive);

                if (existingClaim != null)
                {
                    return BadRequest(new { success = false, message = "You have already claimed this offer" });
                }

                // Generate unique offer code
                string offerCode = GenerateOfferCode();

                // Create new claimed offer
                var claimedOffer = new TblClaimedOffer
                {
                    OfferId = request.OfferId,
                    UserId = request.UserId,
                    ClaimedDate = DateTime.Now,
                    IsUsed = false,
                    IsActive = true,
                    OfferCode = offerCode
                };

                _context.TblClaimedOffer.Add(claimedOffer);
                await _context.SaveChangesAsync();

                // Reload with offer included
                var createdOffer = await _context.TblClaimedOffer
                    .Include(co => co.Offer)
                    .FirstOrDefaultAsync(co => co.ClaimedOfferId == claimedOffer.ClaimedOfferId);

                return Ok(new
                {
                    success = true,
                    message = "Offer claimed successfully",
                    data = MapToClaimedOfferDto(createdOffer)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // PUT: api/TblClaimedOffers/Use/5
        [HttpPut("Use/{claimedOfferId}")]
        public async Task<IActionResult> UseClaimedOffer(int claimedOfferId, [FromBody] UseOfferRequest request)
        {
            try
            {
                var claimedOffer = await _context.TblClaimedOffer
                    .Include(co => co.Offer)
                    .FirstOrDefaultAsync(co => co.ClaimedOfferId == claimedOfferId);

                if (claimedOffer == null)
                {
                    return NotFound(new { success = false, message = "Claimed offer not found" });
                }

                if (claimedOffer.UserId != request.UserId)
                {
                    return Forbid();
                }

                if (claimedOffer.IsUsed)
                {
                    return BadRequest(new { success = false, message = "This offer has already been used" });
                }

                if (!claimedOffer.IsActive)
                {
                    return BadRequest(new { success = false, message = "This offer is no longer active" });
                }

                if (claimedOffer.Offer.ValidUntil < DateTime.Now)
                {
                    return BadRequest(new { success = false, message = "This offer has expired" });
                }

                // Check if the game is included in the offer
                bool isGameIncluded = true;
                if (!string.IsNullOrEmpty(claimedOffer.Offer.GamesIncluded))
                {
                    var gamesIncluded = JsonSerializer.Deserialize<List<string>>(claimedOffer.Offer.GamesIncluded);

                    // Get game name from ID
                    var game = await _context.Games.FindAsync(request.GameId);
                    if (game != null && gamesIncluded != null && gamesIncluded.Any())
                    {
                        isGameIncluded = gamesIncluded.Contains(game.Title);
                    }
                }

                if (!isGameIncluded)
                {
                    return BadRequest(new { success = false, message = "This offer cannot be used for the selected game" });
                }

                // Mark as used
                claimedOffer.IsUsed = true;
                claimedOffer.UsedDate = DateTime.Now;

                _context.Entry(claimedOffer).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Offer used successfully",
                    discountType = claimedOffer.Offer.DiscountType,
                    discountValue = claimedOffer.Offer.DiscountValue,
                    //originalPrice = claimedOffer.Offer.OriginalPrice,
                    //discountedPrice = claimedOffer.Offer.DiscountedPrice
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Helper methods
        private string GenerateOfferCode()
        {
            // Generate a unique code for the claimed offer
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string code = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return $"OFFER-{code}";
        }

        private ClaimedOfferDto MapToClaimedOfferDto(TblClaimedOffer claimedOffer)
        {
            var offer = claimedOffer.Offer;
            var timeLeft = CalculateTimeLeft(offer.ValidUntil);
            var gamesIncluded = !string.IsNullOrEmpty(offer.GamesIncluded)
                ? JsonSerializer.Deserialize<string[]>(offer.GamesIncluded)
                : new string[0];

            return new ClaimedOfferDto
            {
                ClaimedOfferId = claimedOffer.ClaimedOfferId,
                OfferId = claimedOffer.OfferId,
                UserId = claimedOffer.UserId,
                ClaimedDate = claimedOffer.ClaimedDate,
                UsedDate = claimedOffer.UsedDate,
                IsUsed = claimedOffer.IsUsed,
                IsActive = claimedOffer.IsActive,
                OfferCode = claimedOffer.OfferCode,

                // Offer details
                OfferTitle = offer.Title,
                Description = offer.Description,
                DiscountType = offer.DiscountType,
                DiscountValue = offer.DiscountValue,
                //OriginalPrice = offer.OriginalPrice,
                //DiscountedPrice = offer.DiscountedPrice,
                Category = offer.Category,
                ValidUntil = offer.ValidUntil,
                ImageUrl = offer.ImageUrl,
                GamesIncluded = gamesIncluded,
                IsExpired = offer.ValidUntil < DateTime.Now,
                TimeLeft = timeLeft
            };
        }

        private string CalculateTimeLeft(DateTime validUntil)
        {
            var difference = validUntil - DateTime.Now;
            if (difference.TotalDays > 0)
            {
                return $"{(int)difference.TotalDays} days left";
            }
            else if (difference.TotalHours > 0)
            {
                return $"{(int)difference.TotalHours} hours left";
            }
            else if (difference.TotalMinutes > 0)
            {
                return $"{(int)difference.TotalMinutes} minutes left";
            }
            else
            {
                return "Expired";
            }
        }
    }

    public class ClaimOfferRequest
    {
        public int UserId { get; set; }
        public int OfferId { get; set; }
    }

    public class UseOfferRequest
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
    }
}
