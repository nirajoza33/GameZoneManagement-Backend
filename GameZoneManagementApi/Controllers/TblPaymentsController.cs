 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameZoneManagementApi.Models;
using Humanizer;
using GameZoneManagementApi.DTOs;

namespace GameZoneManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblPaymentsController : ControllerBase
    {
        private readonly GamezoneDbContext _context;

        public TblPaymentsController(GamezoneDbContext context)
        {
            _context = context;
        }

        // GET: api/TblPayments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblPayment>>> GetTblPayments()
        {
            return await _context.TblPayments.ToListAsync();
        }

        // GET: api/TblPayments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblPayment>> GetTblPayment(int id)
        {
            var tblPayment = await _context.TblPayments.FindAsync(id);

            if (tblPayment == null)
            {
                return NotFound();
            }

            return tblPayment;
        }

        // PUT: api/TblPayments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblPayment(int id, TblPayment tblPayment)
        {
            if (id != tblPayment.PaymentId)
            {
                return BadRequest();
            }

            _context.Entry(tblPayment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblPaymentExists(id))
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

        // POST: api/TblPayments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblPayment>> PostTblPayment(CreatePaymentDto dto)
        {
            var payment = new TblPayment
            {
                TransactionId = dto.TransactionId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                PaymentDate = dto.PaymentDate,
                GameId = dto.GameId,
                PaymentStatus = PaymentStatus.Completed,
            };

            _context.TblPayments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblPayment", new { id = payment.PaymentId }, payment);
        }

        // DELETE: api/TblPayments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblPayment(int id)
        {
            var tblPayment = await _context.TblPayments.FindAsync(id);
            if (tblPayment == null)
            {
                return NotFound();
            }

            _context.TblPayments.Remove(tblPayment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblPaymentExists(int id)
        {
            return _context.TblPayments.Any(e => e.PaymentId == id);
        }

        // GET: api/TblPayments/RevenueByUserOverTime/{userId}
        [HttpGet("RevenueByUserOverTime/{userId:int}")]
        public async Task<ActionResult<IEnumerable<object>>> GetRevenueByUserOverTime(int userId)
        {
            var revenueData = await (
                from payment in _context.TblPayments
                join game in _context.Games on payment.GameId equals game.GameId
                where game.UserId == userId && payment.PaymentStatus == PaymentStatus.Completed
                group payment by new { payment.PaymentDate.Year, payment.PaymentDate.Month } into grp
                orderby grp.Key.Year, grp.Key.Month
                select new
                {
                    // Create a readable month label, e.g., "Jan 2023"
                    month = new DateTime(grp.Key.Year, grp.Key.Month, 1).ToString("MMM yyyy"),
                    revenue = grp.Sum(x => x.Amount)
                }
            ).ToListAsync();

            return Ok(revenueData);
        }

        [HttpGet("RevenueByUserDaily/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> RevenueByUserDaily(int userId)
        {
            var revenueData = await (
                from payment in _context.TblPayments
                join game in _context.Games on payment.GameId equals game.GameId
                where game.UserId == userId && payment.PaymentStatus == PaymentStatus.Completed
                group payment by payment.PaymentDate.Date into grp
                orderby grp.Key
                select new
                {
                    date = grp.Key.ToString("yyyy-MM-dd"),
                    revenue = grp.Sum(x => x.Amount)
                }
            ).ToListAsync();

            return Ok(revenueData);
        }



        [HttpGet("RevenueByGame/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetRevenueByGame(int userId)
        {
            try
            {
                var gameRevenue = await (
                    from payment in _context.TblPayments
                    join game in _context.Games on payment.GameId equals game.GameId
                    where game.UserId == userId && payment.PaymentStatus == PaymentStatus.Completed
                    group payment by new { game.GameId, game.Title, game.Category.CategoryName } into grp
                    select new
                    {
                        gameId = grp.Key.GameId,
                        name = grp.Key.Title,
                        category = grp.Key.CategoryName ?? "Unknown",
                        revenue = grp.Sum(x => x.Amount),
                        players = grp.Select(x => x.UserId).Distinct().Count(),
                        downloads = grp.Count() + new Random().Next(10, 100) // Simulated downloads
                    }
                ).ToListAsync();

                return Ok(gameRevenue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }



    }
}
