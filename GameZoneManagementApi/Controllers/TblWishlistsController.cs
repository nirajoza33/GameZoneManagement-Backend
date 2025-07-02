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
    public class TblWishlistsController : ControllerBase
    {
        private readonly GamezoneDbContext _context;

        public TblWishlistsController(GamezoneDbContext context)
        {
            _context = context;
        }

        // GET: api/TblWishlists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblWishlist>>> GetTblWishlists()
        {
            return await _context.TblWishlists.ToListAsync();
        }

        // GET: api/TblWishlists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblWishlist>> GetTblWishlist(int id)
        {
            var tblWishlist = await _context.TblWishlists.FindAsync(id);

            if (tblWishlist == null)
            {
                return NotFound();
            }

            return tblWishlist;
        }

        // PUT: api/TblWishlists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblWishlist(int id, WishlistDto wishlistDto)
        {
            var wishlist = await _context.TblWishlists.FindAsync(id);
            if (id != wishlist.WishlistId)
            {
                return BadRequest();
            }

            _context.Entry(wishlist).State = EntityState.Modified;

            try
            {
                wishlist.GameId = wishlistDto.GameId;
                wishlist.UserId = wishlistDto.UserId;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblWishlistExists(id))
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

        // POST: api/TblWishlists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblWishlist>> PostTblWishlist(WishlistDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is null");

            var wishlistItem = new TblWishlist
            {
                UserId = dto.UserId,
                GameId = dto.GameId
            };

            _context.TblWishlists.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblWishlist", new { id = wishlistItem.WishlistId }, wishlistItem);
        }

        // DELETE: api/TblWishlists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblWishlist(int id)
        {
            var tblWishlist = await _context.TblWishlists.FindAsync(id);
            if (tblWishlist == null)
            {
                return NotFound();
            }

            _context.TblWishlists.Remove(tblWishlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblWishlistExists(int id)
        {
            return _context.TblWishlists.Any(e => e.WishlistId == id);
        }
    }
}
