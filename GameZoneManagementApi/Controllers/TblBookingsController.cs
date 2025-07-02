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
//    public class TblBookingsController : ControllerBase
//    {
//        private readonly GamezoneDbContext _context;

//        public TblBookingsController(GamezoneDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/TblBookings
//        [HttpGet]
//        //public async Task<ActionResult<IEnumerable<TblBooking>>> GetTblBookings()
//        //{
//        //    return await _context.TblBookings.ToListAsync();
//        //}
//        public async Task<ActionResult<IEnumerable<TblBooking>>> GetTblBookings([FromQuery] int? userId)
//        {
//            IQueryable<TblBooking> query = _context.TblBookings
//                .Include(b => b.Game) // Optional: include related game data
//                .Include(b => b.User); // Optional: include related user data

//            if (userId.HasValue)
//            {
//                query = query.Where(b => b.UserId == userId.Value);
//            }

//            var bookings = await query.ToListAsync();

//            return bookings;
//        }

//        // GET: api/TblBookings/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<TblBooking>> GetTblBooking(int id)
//        {
//            var tblBooking = await _context.TblBookings.FindAsync(id);

//            if (tblBooking == null)
//            {
//                return NotFound();
//            }

//            return tblBooking;
//        }

//        // PUT: api/TblBookings/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutTblBooking(int id, UpdateBookingDto dto)
//        {
//            if (id != dto.BookingId)
//            {
//                return BadRequest();
//            }

//            var booking = await _context.TblBookings.FindAsync(id);

//            _context.Entry(booking).State = EntityState.Modified;

//            try
//            {
//                // Update fields
//                booking.GameId = dto.GameId;
//                booking.UserId = dto.UserId;
//                booking.Price = dto.Price;
//                booking.Status = dto.Status;
//                booking.BookingDate = dto.BookingDate;
//                booking.StartTime = dto.StartTime;
//                booking.EndTime = dto.EndTime;

//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!TblBookingExists(id))
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

//        // POST: api/TblBookings
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<TblBooking>> PostTblBooking(CreateBookingDto dto)
//        {

//            // Validate GameId
//            var gameExists = await _context.Games.AnyAsync(g => g.GameId == dto.GameId);
//            if (!gameExists)
//            {
//                return BadRequest($"Game with ID {dto.GameId} does not exist.");
//            }

//            // Validate UserId
//            var userExists = await _context.Tblusers.AnyAsync(u => u.UserId == dto.UserId);
//            if (!userExists)
//            {
//                return BadRequest($"User with ID {dto.UserId} does not exist.");
//            }

//            var booking = new TblBooking
//            {
//                GameId = dto.GameId,
//                UserId = dto.UserId,
//                Price = dto.Price,
//                BookingDate = dto.BookingDate.Date, // Ensures only the date part is used
//                StartTime = dto.StartTime,
//                EndTime = dto.EndTime,
//                Status = BookingStatus.Confirmed
//            };

//            _context.TblBookings.Add(booking);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetTblBooking", new { id = booking.BookingId }, booking);
//        }

//        // DELETE: api/TblBookings/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteTblBooking(int id)
//        {
//            var tblBooking = await _context.TblBookings.FindAsync(id);
//            if (tblBooking == null)
//            {
//                return NotFound();
//            }

//            _context.TblBookings.Remove(tblBooking);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool TblBookingExists(int id)
//        {
//            return _context.TblBookings.Any(e => e.BookingId == id);
//        }
//    }
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

namespace GameZoneManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblBookingsController : ControllerBase
    {
        private readonly GamezoneDbContext _context;

        public TblBookingsController(GamezoneDbContext context)
        {
            _context = context;
        }

        // GET: api/TblBookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblBooking>>> GetTblBookings([FromQuery] int? userId, [FromQuery] bool? withOffers)
        {
            IQueryable<TblBooking> query = _context.TblBookings
                .Include(b => b.Game)
                .Include(b => b.User);

            if (userId.HasValue)
            {
                query = query.Where(b => b.UserId == userId.Value);
            }

            // Filter bookings with offers if requested
            if (withOffers.HasValue && withOffers.Value)
            {
                query = query.Where(b => b.HasOfferApplied);
            }

            var bookings = await query.ToListAsync();
            return bookings;
        }

        // GET: api/TblBookings/WithOffers
        [HttpGet("WithOffers")]
        public async Task<ActionResult<IEnumerable<BookingWithOfferDto>>> GetBookingsWithOffers([FromQuery] int? ownerId)
        {
            // Start with bookings that have offers applied
            var query = _context.TblBookings
                .Where(b => b.HasOfferApplied)
                .Include(b => b.Game)
                .Include(b => b.User)
                .AsQueryable();

            // If owner ID is provided, filter by games owned by that user
            if (ownerId.HasValue)
            {
                query = query.Where(b => b.Game.UserId == ownerId.Value);
            }

            var bookingsWithOffers = await query.Select(b => new BookingWithOfferDto
            {
                BookingId = b.BookingId,
                GameId = b.GameId,
                GameTitle = b.Game.Title,
                GameCategory = b.Game.Category,
                UserId = b.UserId,
                UserName = b.User.UserName,
                OriginalPrice = b.OriginalPrice,
                FinalPrice = b.Price,
                DiscountAmount = b.DiscountAmount ?? 0,
                SavingsPercentage = b.OriginalPrice > 0 ?
                    ((b.OriginalPrice - b.Price) / b.OriginalPrice) * 100 : 0,
                BookingDate = b.BookingDate,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status,
                AppliedOfferId = b.AppliedOfferId,
                OfferCode = b.OfferCode,
                // Join with Offers table to get offer details
                OfferTitle = _context.TblOffers
                    .Where(o => o.OfferId == b.AppliedOfferId)
                    .Select(o => o.Title)
                    .FirstOrDefault() ?? "Unknown Offer",
                OfferCategory = _context.TblOffers
                    .Where(o => o.OfferId == b.AppliedOfferId)
                    .Select(o => o.Category)
                    .FirstOrDefault() ?? "General"
            }).ToListAsync();

            return bookingsWithOffers;
        }

        // GET: api/TblBookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblBooking>> GetTblBooking(int id)
        {
            var tblBooking = await _context.TblBookings
                .Include(b => b.Game)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (tblBooking == null)
            {
                return NotFound();
            }

            return tblBooking;
        }

        // PUT: api/TblBookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblBooking(int id, UpdateBookingDto dto)
        {
            if (id != dto.BookingId)
            {
                return BadRequest();
            }

            var booking = await _context.TblBookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Update basic fields
            booking.GameId = dto.GameId;
            booking.UserId = dto.UserId;
            booking.BookingDate = dto.BookingDate;
            booking.StartTime = dto.StartTime;
            booking.EndTime = dto.EndTime;
            booking.Status = dto.Status;

            // Update offer-related fields
            booking.OriginalPrice = dto.OriginalPrice;
            booking.Price = dto.Price;
            booking.HasOfferApplied = dto.HasOfferApplied;
            booking.AppliedOfferId = dto.AppliedOfferId;
            booking.DiscountAmount = dto.DiscountAmount;
            booking.OfferCode = dto.OfferCode;

            // If an offer was applied, update the claimed offer record
            if (booking.HasOfferApplied && booking.AppliedOfferId.HasValue)
            {
                var claimedOffer = await _context.TblClaimedOffers
                    .FirstOrDefaultAsync(co =>
                        co.OfferId == booking.AppliedOfferId &&
                        co.UserId == booking.UserId &&
                        co.BookingId == booking.BookingId);

                if (claimedOffer != null)
                {
                    claimedOffer.IsUsed = true;
                    claimedOffer.UsedDate = DateTime.Now;
                    claimedOffer.ActualDiscountApplied = booking.DiscountAmount ?? 0;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblBookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TblBookings
        [HttpPost]
        public async Task<ActionResult<TblBooking>> PostTblBooking(CreateBookingDto dto)
        {
            // Validate GameId
            var gameExists = await _context.Games.AnyAsync(g => g.GameId == dto.GameId);
            if (!gameExists)
            {
                return BadRequest($"Game with ID {dto.GameId} does not exist.");
            }

            // Validate UserId
            var userExists = await _context.Tblusers.AnyAsync(u => u.UserId == dto.UserId);
            if (!userExists)
            {
                return BadRequest($"User with ID {dto.UserId} does not exist.");
            }

            // Validate offer if one is being applied
            if (dto.HasOfferApplied && dto.AppliedOfferId.HasValue)
            {
                var offer = await _context.TblOffers.FindAsync(dto.AppliedOfferId.Value);
                if (offer == null)
                {
                    return BadRequest($"Offer with ID {dto.AppliedOfferId.Value} does not exist.");
                }

                // Check if the offer has been claimed by this user
                var claimedOffer = await _context.TblClaimedOffers
                    .FirstOrDefaultAsync(co =>
                        co.OfferId == dto.AppliedOfferId.Value &&
                        co.UserId == dto.UserId &&
                        co.IsActive &&
                        !co.IsUsed);

                if (claimedOffer == null)
                {
                    return BadRequest("This offer has not been claimed by the user or has already been used.");
                }
            }

            var booking = new TblBooking
            {
                GameId = dto.GameId,
                UserId = dto.UserId,
                BookingDate = dto.BookingDate.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = BookingStatus.Confirmed,

                // Offer-related fields
                OriginalPrice = dto.OriginalPrice,
                Price = dto.Price,
                HasOfferApplied = dto.HasOfferApplied,
                AppliedOfferId = dto.AppliedOfferId,
                DiscountAmount = dto.DiscountAmount,
                OfferCode = dto.OfferCode
            };

            _context.TblBookings.Add(booking);
            await _context.SaveChangesAsync();

            // If an offer was applied, update the claimed offer record
            if (booking.HasOfferApplied && booking.AppliedOfferId.HasValue)
            {
                var claimedOffer = await _context.TblClaimedOffers
                    .FirstOrDefaultAsync(co =>
                        co.OfferId == booking.AppliedOfferId.Value &&
                        co.UserId == booking.UserId &&
                        co.IsActive &&
                        !co.IsUsed);

                if (claimedOffer != null)
                {
                    claimedOffer.IsUsed = true;
                    claimedOffer.UsedDate = DateTime.Now;
                    claimedOffer.BookingId = booking.BookingId;
                    claimedOffer.ActualDiscountApplied = booking.DiscountAmount ?? 0;

                    await _context.SaveChangesAsync();
                }
            }

            return CreatedAtAction("GetTblBooking", new { id = booking.BookingId }, booking);
        }

        // DELETE: api/TblBookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblBooking(int id)
        {
            var tblBooking = await _context.TblBookings.FindAsync(id);
            if (tblBooking == null)
            {
                return NotFound();
            }

            // If this booking used an offer, update the claimed offer record
            if (tblBooking.HasOfferApplied && tblBooking.AppliedOfferId.HasValue)
            {
                var claimedOffer = await _context.TblClaimedOffers
                    .FirstOrDefaultAsync(co =>
                        co.BookingId == tblBooking.BookingId);

                if (claimedOffer != null)
                {
                    // Reset the claimed offer so it can be used again
                    claimedOffer.IsUsed = false;
                    claimedOffer.UsedDate = null;
                    claimedOffer.BookingId = null;
                    claimedOffer.ActualDiscountApplied = 0;
                }
            }

            _context.TblBookings.Remove(tblBooking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblBookingExists(int id)
        {
            return _context.TblBookings.Any(e => e.BookingId == id);
        }
    }
}
