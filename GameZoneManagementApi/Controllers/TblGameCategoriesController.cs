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
    public class TblGameCategoriesController : ControllerBase
    {
        private readonly GamezoneDbContext _context;

        public TblGameCategoriesController(GamezoneDbContext context)
        {
            _context = context;
        }

        // GET: api/TblGameCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblGameCategory>>> GetGameCategory()
        {
            return await _context.GameCategory.ToListAsync();
        }

        // GET: api/TblGameCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblGameCategory>> GetTblGameCategory(int id)
        {
            var tblGameCategory = await _context.GameCategory.FindAsync(id);

            if (tblGameCategory == null)
            {
                return NotFound();
            }

            return tblGameCategory;
        }

        // PUT: api/TblGameCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblGameCategory(int id, CreateGameCategoryDto createGameCategoryDto)
        {
            var gameCategory = await _context.GameCategory.FindAsync(id);
            if (id != gameCategory.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(gameCategory).State = EntityState.Modified;

            try
            {
                if(gameCategory == null) { return NotFound(); }

                gameCategory.CategoryName = createGameCategoryDto.CategoryName;
                gameCategory.Description = createGameCategoryDto.Description;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblGameCategoryExists(id))
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

        // POST: api/TblGameCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblGameCategory>> PostTblGameCategory(CreateGameCategoryDto createGameCategoryDto)
        {
            if (_context.GameCategory.Any(c => c.CategoryName.ToLower() == createGameCategoryDto.CategoryName.ToLower()))
            {
                return Conflict(new { message = "Category name already exists." });
            }

            var gameCategory = new TblGameCategory
            {
                CategoryName = createGameCategoryDto.CategoryName,
                Description = createGameCategoryDto.Description,
            };
            _context.GameCategory.Add(gameCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblGameCategory", new { id = gameCategory.CategoryId }, gameCategory);
        }

        // DELETE: api/TblGameCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblGameCategory(int id)
        {
            var tblGameCategory = await _context.GameCategory.FindAsync(id);
            if (tblGameCategory == null)
            {
                return NotFound();
            }

            _context.GameCategory.Remove(tblGameCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblGameCategoryExists(int id)
        {
            return _context.GameCategory.Any(e => e.CategoryId == id);
        }









    }
}
