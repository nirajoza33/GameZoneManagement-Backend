using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameZoneManagementApi.Models;
using GameZoneManagementApi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace GameZoneManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblGamesController : ControllerBase
    {
        private readonly GamezoneDbContext _context;

        public TblGamesController(GamezoneDbContext context)
        {
            _context = context;
        }

        //// GET: api/  
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TblGame>>> GetGames()
        //{
        //    return await _context.Games.ToListAsync();
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetGames()
        {
            var games = await _context.Games
                .Include(g => g.Category) // Ensure navigation property is included
                .Select(g => new
                {
                    g.GameId,
                    g.Title,
                    g.Description,
                    g.Pricing,
                    g.ImageUrl,
                    g.UserId,
                    g.CategoryId,
                    g.Status,
                    Cname = g.Category != null ? g.Category.CategoryName : "Unknown"
                })
                .ToListAsync();

            return Ok(games);
        }


        // GET: api/TblGames/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblGame>> GetTblGame(int id)
        {
            var tblGame = await _context.Games.FindAsync(id);

            if (tblGame == null)
            {
                return NotFound();
            }

            return tblGame;
        }

        // PUT: api/TblGames/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblGame(int id, [FromForm] UpdateGameDto dto)
        {
            var game = await _context.Games.FindAsync(id);

            if (id != game.GameId)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                if (game == null) return NotFound();

                game.Title = dto.Title;
                game.Description = dto.Description;
                game.Pricing = dto.Pricing;
                game.UserId = dto.UserId;
                game.CategoryId = dto.CategoryId;
                game.Status = dto.Status;
                //game.ImageUrl = dto.ImageFile;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblGameExists(id))
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

        // POST: api/TblGames
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<TblGame>> PostTblGame([FromForm] CreateGameDto dto)
        //{
        //    var tblGame = new TblGame
        //    {
        //        Title = dto.Title,
        //        Description = dto.Description,
        //        Pricing = dto.Pricing,
        //        GameZoneOwnerId = dto.GameZoneOwnerId,
        //        ImageUrl = dto.ImageUrl 
        //    };

        //    _context.Games.Add(tblGame);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTblGame", new { id = tblGame.GameId }, tblGame);
        //}

        [HttpPost]
        //[Authorize(Roles = "GameZoneOwner")]    
        public async Task<ActionResult<TblGame>> PostTblGame([FromForm] CreateGameDto dto)
        {
            var imageFile = dto.ImageFile;

            // Check for file existence
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest(new { success = false, message = "Image file is required." });

            // Check file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExt = Path.GetExtension(imageFile.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExt))
                return BadRequest(new { success = false, message = "Only .jpg, .jpeg, .png formats are allowed." });

            // Check file size
            const int maxFileSize = 2 * 1024 * 1024;
            if (imageFile.Length > maxFileSize)
                return BadRequest(new { success = false, message = "Image size must be less than 2MB." });

            // Ensure uploads folder exists
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            // Generate a unique file name
            var uniqueFileName = $"{Guid.NewGuid()}{fileExt}";
            var savedPath = Path.Combine(uploadsPath, uniqueFileName);

            // Save the image to the upload folder
            using (var fileStream = new FileStream(savedPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Save only the file name in DB
            var game = new TblGame
            {
                Title = dto.Title,
                Description = dto.Description,
                Pricing = dto.Pricing,
                //GameZoneOwnerId = dto.GameZoneOwnerId,
                UserId = dto.UserId,
                CategoryId = dto.CategoryId,
                ImageUrl = uniqueFileName
                //ImageUrl = uniqueFileName // ← only the file name
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblGame", new { id = game.GameId }, game);
        }


        [HttpPost("UpdateGameWithImage")]
        public async Task<IActionResult> UpdateGameWithImage([FromForm] UpdateGameDto dto)
        {
            var game = await _context.Games.FindAsync(dto.GameId);
            if (game == null)
                return NotFound();

            game.Title = dto.Title;
            game.Description = dto.Description;
            game.Pricing = dto.Pricing;
            game.UserId = dto.UserId;
            game.CategoryId = dto.CategoryId;

            if (dto.ImageFile != null)
            {
                var fileExt = Path.GetExtension(dto.ImageFile.FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                if (!allowedExtensions.Contains(fileExt))
                    return BadRequest("Invalid file format.");

                var fileName = $"{Guid.NewGuid()}{fileExt}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                // Optional: delete old image file (if needed)
                // var oldPath = Path.Combine("wwwroot/uploads", game.ImageUrl);
                // if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);

                game.ImageUrl = fileName;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Game updated successfully." });
        }



        // DELETE: api/TblGames/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblGame(int id)
        {
            var tblGame = await _context.Games.FindAsync(id);
            if (tblGame == null)
            {
                return NotFound();
            }

            _context.Games.Remove(tblGame);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblGameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }


        [HttpGet("ByUser/{userId:int}")]
        public async Task<ActionResult<IEnumerable<object>>> GetGamesByUser(int userId)
        {
            var games = await _context.Games
                .Include(g => g.Category)
                .Where(g => g.UserId == userId)
                .Select(g => new
                {
                    g.GameId,
                    g.Title,
                    g.Description,
                    g.Pricing,
                    g.ImageUrl,
                    g.UserId,
                    g.CategoryId,
                    g.Status,
                    Cname = g.Category != null ? g.Category.CategoryName : "Unknown"
                })
                .ToListAsync();

            return Ok(games);
        }

        // GET: api/TblGames/TotalRevenueByUser/5
        [HttpGet("TotalRevenueByUser/{userId:int}")]
        public async Task<ActionResult<decimal>> GetTotalRevenueByUser(int userId)
        {
            var totalRevenue = await _context.Games
                .Where(g => g.UserId == userId)
                .SumAsync(g => g.Pricing);

            return Ok(totalRevenue);
        }



    }
}




