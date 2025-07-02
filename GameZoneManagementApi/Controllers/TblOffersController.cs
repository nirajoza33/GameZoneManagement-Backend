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
//    public class TblOffersController : ControllerBase
//    {
//        private readonly GamezoneDbContext _context;

//        public TblOffersController(GamezoneDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/TblOffers
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<TblOffer>>> GetTblOffers()
//        {
//            return await _context.TblOffers.ToListAsync();
//        }

//        // GET: api/TblOffers/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<TblOffer>> GetTblOffer(int id)
//        {
//            var tblOffer = await _context.TblOffers.FindAsync(id);

//            if (tblOffer == null)
//            {
//                return NotFound();
//            }

//            return tblOffer;
//        }

//        // PUT: api/TblOffers/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutTblOffer(int id, TblOffer tblOffer)
//        {
//            if (id != tblOffer.OfferId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(tblOffer).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!TblOfferExists(id))
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

//        // POST: api/TblOffers
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<TblOffer>> PostTblOffer(TblOffer tblOffer)
//        {
//            _context.TblOffers.Add(tblOffer);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetTblOffer", new { id = tblOffer.OfferId }, tblOffer);
//        }

//        // DELETE: api/TblOffers/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteTblOffer(int id)
//        {
//            var tblOffer = await _context.TblOffers.FindAsync(id);
//            if (tblOffer == null)
//            {
//                return NotFound();
//            }

//            _context.TblOffers.Remove(tblOffer);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool TblOfferExists(int id)
//        {
//            return _context.TblOffers.Any(e => e.OfferId == id);
//        }
//    }
//}


//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using GameZoneManagementApi.Models;
//using GameZoneManagementApi.DTOs;
//using System.Text.Json;
//using Microsoft.AspNetCore.Authorization;

//namespace GameZoneManagementApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TblOffersController : ControllerBase
//    {
//        private readonly GamezoneDbContext _context;

//        public TblOffersController(GamezoneDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/TblOffers
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetOffers()
//        {
//            var offers = await _context.TblOffers
//                .Include(o => o.User)
//                .Where(o => o.Status && o.ValidUntil >= DateTime.Now)
//                .OrderByDescending(o => o.IsFeatured)
//                .ThenByDescending(o => o.IsTrending)
//                .ThenByDescending(o => o.CreatedDate)
//                .ToListAsync();

//            var response = offers.Select(o => MapToResponseDto(o)).ToList();
//            return Ok(response);
//        }

//        // GET: api/TblOffers/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<OfferResponseDto>> GetOffer(int id)
//        {
//            var offer = await _context.TblOffers
//                .Include(o => o.User)
//                .FirstOrDefaultAsync(o => o.OfferId == id);

//            if (offer == null)
//            {
//                return NotFound();
//            }

//            return Ok(MapToResponseDto(offer));
//        }

//        // GET: api/TblOffers/ByUser/5
//        [HttpGet("ByUser/{userId:int}")]
//        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetOffersByUser(int userId)
//        {
//            var offers = await _context.TblOffers
//                .Include(o => o.User)
//                .Where(o => o.UserId == userId)
//                .OrderByDescending(o => o.CreatedDate)
//                .ToListAsync();

//            var response = offers.Select(o => MapToResponseDto(o)).ToList();
//            return Ok(response);
//        }

//        // GET: api/TblOffers/ByCategory/weekend
//        [HttpGet("ByCategory/{category}")]
//        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetOffersByCategory(string category)
//        {
//            var offers = await _context.TblOffers
//                .Include(o => o.User)
//                .Where(o => o.Category.ToLower() == category.ToLower() &&
//                           o.Status &&
//                           o.ValidUntil >= DateTime.Now)
//                .OrderByDescending(o => o.IsFeatured)
//                .ThenByDescending(o => o.IsTrending)
//                .ToListAsync();

//            var response = offers.Select(o => MapToResponseDto(o)).ToList();
//            return Ok(response);
//        }

//        // GET: api/TblOffers/Featured
//        [HttpGet("Featured")]
//        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetFeaturedOffers()
//        {
//            var offers = await _context.TblOffers
//                .Include(o => o.User)
//                .Where(o => o.IsFeatured && o.Status && o.ValidUntil >= DateTime.Now)
//                .OrderByDescending(o => o.CreatedDate)
//                .ToListAsync();

//            var response = offers.Select(o => MapToResponseDto(o)).ToList();
//            return Ok(response);
//        }

//        // GET: api/TblOffers/Trending
//        [HttpGet("Trending")]
//        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetTrendingOffers()
//        {
//            var offers = await _context.TblOffers
//                .Include(o => o.User)
//                .Where(o => o.IsTrending && o.Status && o.ValidUntil >= DateTime.Now)
//                .OrderByDescending(o => o.CreatedDate)
//                .ToListAsync();

//            var response = offers.Select(o => MapToResponseDto(o)).ToList();
//            return Ok(response);
//        }

//        // POST: api/TblOffers
//        [HttpPost]
//        //[Authorize(Roles = "GameZoneOwner")]
//        public async Task<ActionResult<OfferResponseDto>> CreateOffer([FromForm] CreateOfferDto dto)
//        {
//            try
//            {
//                var offer = new TblOffer
//                {
//                    Title = dto.Title,
//                    Description = dto.Description,
//                    DiscountType = dto.DiscountType,
//                    DiscountValue = dto.DiscountValue,
//                    OriginalPrice = dto.OriginalPrice,
//                    DiscountedPrice = dto.DiscountedPrice,
//                    Category = dto.Category,
//                    ValidFrom = dto.ValidFrom,
//                    ValidUntil = dto.ValidUntil,
//                    IsFeatured = dto.IsFeatured,
//                    IsTrending = dto.IsTrending,
//                    Icon = dto.Icon,
//                    GradientClass = dto.GradientClass,
//                    AccentColor = dto.AccentColor,
//                    GamesIncluded = dto.GamesIncluded != null ? JsonSerializer.Serialize(dto.GamesIncluded) : null,
//                    Terms = dto.Terms != null ? JsonSerializer.Serialize(dto.Terms) : null,
//                    UserId = dto.UserId,
//                    Status = dto.Status,
//                    CreatedDate = DateTime.Now
//                };

//                // Handle image upload
//                if (dto.ImageFile != null && dto.ImageFile.Length > 0)
//                {
//                    var imageUrl = await SaveImageAsync(dto.ImageFile);
//                    if (imageUrl != null)
//                    {
//                        offer.ImageUrl = imageUrl;
//                    }
//                }

//                _context.TblOffers.Add(offer);
//                await _context.SaveChangesAsync();

//                var createdOffer = await _context.TblOffers
//                    .Include(o => o.User)
//                    .FirstOrDefaultAsync(o => o.OfferId == offer.OfferId);

//                return CreatedAtAction(nameof(GetOffer), new { id = offer.OfferId }, MapToResponseDto(createdOffer!));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { success = false, message = ex.Message });
//            }
//        }

//        // PUT: api/TblOffers/5
//        [HttpPut("{id}")]
//        //[Authorize(Roles = "GameZoneOwner")]
//        public async Task<IActionResult> UpdateOffer(int id, [FromForm] UpdateOfferDto dto)
//        {
//            if (id != dto.OfferId)
//            {
//                return BadRequest("Offer ID mismatch");
//            }

//            var offer = await _context.TblOffers.FindAsync(id);
//            if (offer == null)
//            {
//                return NotFound();
//            }

//            // Check if the user owns this offer
//            if (offer.UserId != dto.UserId)
//            {
//                return Forbid("You can only update your own offers");
//            }

//            try
//            {
//                offer.Title = dto.Title;
//                offer.Description = dto.Description;
//                offer.DiscountType = dto.DiscountType;
//                offer.DiscountValue = dto.DiscountValue;
//                offer.OriginalPrice = dto.OriginalPrice;
//                offer.DiscountedPrice = dto.DiscountedPrice;
//                offer.Category = dto.Category;
//                offer.ValidFrom = dto.ValidFrom;
//                offer.ValidUntil = dto.ValidUntil;
//                offer.IsFeatured = dto.IsFeatured;
//                offer.IsTrending = dto.IsTrending;
//                offer.Icon = dto.Icon;
//                offer.GradientClass = dto.GradientClass;
//                offer.AccentColor = dto.AccentColor;
//                offer.GamesIncluded = dto.GamesIncluded != null ? JsonSerializer.Serialize(dto.GamesIncluded) : null;
//                offer.Terms = dto.Terms != null ? JsonSerializer.Serialize(dto.Terms) : null;
//                offer.Status = dto.Status;
//                offer.UpdatedDate = DateTime.Now;

//                // Handle image upload
//                if (dto.ImageFile != null && dto.ImageFile.Length > 0)
//                {
//                    var imageUrl = await SaveImageAsync(dto.ImageFile);
//                    if (imageUrl != null)
//                    {
//                        offer.ImageUrl = imageUrl;
//                    }
//                }

//                _context.Entry(offer).State = EntityState.Modified;
//                await _context.SaveChangesAsync();

//                return NoContent();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!OfferExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { success = false, message = ex.Message });
//            }
//        }

//        // DELETE: api/TblOffers/5
//        [HttpDelete("{id}")]
//        //[Authorize(Roles = "GameZoneOwner")]
//        public async Task<IActionResult> DeleteOffer(int id)
//        {
//            var offer = await _context.TblOffers.FindAsync(id);
//            if (offer == null)
//            {
//                return NotFound();
//            }

//            _context.TblOffers.Remove(offer);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        // GET: api/TblOffers/Stats/5
//        [HttpGet("Stats/{userId:int}")]
//        public async Task<ActionResult<object>> GetOfferStats(int userId)
//        {
//            var totalOffers = await _context.TblOffers.CountAsync(o => o.UserId == userId);
//            var activeOffers = await _context.TblOffers.CountAsync(o => o.UserId == userId && o.Status && o.ValidUntil >= DateTime.Now);
//            var featuredOffers = await _context.TblOffers.CountAsync(o => o.UserId == userId && o.IsFeatured);
//            var expiredOffers = await _context.TblOffers.CountAsync(o => o.UserId == userId && o.ValidUntil < DateTime.Now);

//            return Ok(new
//            {
//                TotalOffers = totalOffers,
//                ActiveOffers = activeOffers,
//                FeaturedOffers = featuredOffers,
//                ExpiredOffers = expiredOffers
//            });
//        }

//        private bool OfferExists(int id)
//        {
//            return _context.TblOffers.Any(e => e.OfferId == id);
//        }

//        private async Task<string?> SaveImageAsync(IFormFile imageFile)
//        {
//            try
//            {
//                // Check file extension
//                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
//                var fileExt = Path.GetExtension(imageFile.FileName).ToLower();
//                if (!allowedExtensions.Contains(fileExt))
//                    return null;

//                // Check file size (max 5MB)
//                const int maxFileSize = 5 * 1024 * 1024;
//                if (imageFile.Length > maxFileSize)
//                    return null;

//                // Ensure uploads folder exists
//                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "offers");
//                if (!Directory.Exists(uploadsPath))
//                    Directory.CreateDirectory(uploadsPath);

//                // Generate unique filename
//                var uniqueFileName = $"{Guid.NewGuid()}{fileExt}";
//                var savedPath = Path.Combine(uploadsPath, uniqueFileName);

//                // Save file
//                using (var fileStream = new FileStream(savedPath, FileMode.Create))
//                {
//                    await imageFile.CopyToAsync(fileStream);
//                }

//                return $"offers/{uniqueFileName}";
//            }
//            catch
//            {
//                return null;
//            }
//        }

//        private OfferResponseDto MapToResponseDto(TblOffer offer)
//        {
//            var timeLeft = CalculateTimeLeft(offer.ValidUntil);
//            var gamesIncluded = !string.IsNullOrEmpty(offer.GamesIncluded)
//                ? JsonSerializer.Deserialize<List<string>>(offer.GamesIncluded)
//                : new List<string>();
//            var terms = !string.IsNullOrEmpty(offer.Terms)
//                ? JsonSerializer.Deserialize<List<string>>(offer.Terms)
//                : new List<string>();

//            return new OfferResponseDto
//            {
//                OfferId = offer.OfferId,
//                Title = offer.Title,
//                Description = offer.Description,
//                DiscountType = offer.DiscountType,
//                DiscountValue = offer.DiscountValue,
//                OriginalPrice = offer.OriginalPrice,
//                DiscountedPrice = offer.DiscountedPrice,
//                Category = offer.Category,
//                ValidFrom = offer.ValidFrom,
//                ValidUntil = offer.ValidUntil,
//                IsFeatured = offer.IsFeatured,
//                IsTrending = offer.IsTrending,
//                ImageUrl = offer.ImageUrl,
//                Icon = offer.Icon,
//                GradientClass = offer.GradientClass,
//                AccentColor = offer.AccentColor,
//                GamesIncluded = gamesIncluded,
//                Terms = terms,
//                UserId = offer.UserId,
//                UserName = offer.User?.UserName,
//                Status = offer.Status,
//                CreatedDate = offer.CreatedDate,
//                UpdatedDate = offer.UpdatedDate,
//                TimeLeft = timeLeft,
//                SavingsAmount = offer.OriginalPrice - offer.DiscountedPrice,
//                IsExpired = offer.ValidUntil < DateTime.Now
//            };
//        }

//        private string CalculateTimeLeft(DateTime validUntil)
//        {
//            var difference = validUntil - DateTime.Now;
//            if (difference.TotalDays > 0)
//            {
//                return $"{(int)difference.TotalDays} days left";
//            }
//            else if (difference.TotalHours > 0)
//            {
//                return $"{(int)difference.TotalHours} hours left";
//            }
//            else if (difference.TotalMinutes > 0)
//            {
//                return $"{(int)difference.TotalMinutes} minutes left";
//            }
//            else
//            {
//                return "Expired";
//            }
//        }
//    }
//}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using GameZoneManagementApi.Models;
//using GameZoneManagementApi.DTOs;
//using System.Text.Json;
//using Microsoft.AspNetCore.Authorization;

//namespace GameZoneManagementApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TblOffersController : ControllerBase
//    {
//        private readonly GamezoneDbContext _context;
//        private readonly IWebHostEnvironment _environment;

//        public TblOffersController(GamezoneDbContext context, IWebHostEnvironment environment)
//        {
//            _context = context;
//            _environment = environment;
//        }

//        // GET: api/TblOffers
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetOffers()
//        {
//            try
//            {
//                var offers = await _context.TblOffers
//                    .Include(o => o.User)
//                    .Where(o => o.Status && o.ValidUntil >= DateTime.Now)
//                    .OrderByDescending(o => o.IsFeatured)
//                    .ThenByDescending(o => o.IsTrending)
//                    .ThenByDescending(o => o.CreatedDate)
//                    .ToListAsync();

//                var response = offers.Select(o => MapToResponseDto(o)).ToList();
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message });
//            }
//        }

//        // GET: api/TblOffers/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<OfferResponseDto>> GetOffer(int id)
//        {
//            try
//            {
//                var offer = await _context.TblOffers
//                    .Include(o => o.User)
//                    .FirstOrDefaultAsync(o => o.OfferId == id);

//                if (offer == null)
//                {
//                    return NotFound(new { success = false, message = "Offer not found" });
//                }

//                return Ok(MapToResponseDto(offer));
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message });
//            }
//        }

//        // GET: api/TblOffers/ByUser/5 (Back to int)
//        [HttpGet("ByUser/{userId:int}")]
//        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetOffersByUser(int userId)
//        {
//            try
//            {
//                var offers = await _context.TblOffers
//                    .Include(o => o.User)
//                    .Where(o => o.UserId == userId)
//                    .OrderByDescending(o => o.CreatedDate)
//                    .ToListAsync();

//                var response = offers.Select(o => MapToResponseDto(o)).ToList();
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message });
//            }
//        }

//        // GET: api/TblOffers/ByCategory/weekend
//        [HttpGet("ByCategory/{category}")]
//        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetOffersByCategory(string category)
//        {
//            try
//            {
//                var offers = await _context.TblOffers
//                    .Include(o => o.User)
//                    .Where(o => o.Category.ToLower() == category.ToLower() &&
//                               o.Status &&
//                               o.ValidUntil >= DateTime.Now)
//                    .OrderByDescending(o => o.IsFeatured)
//                    .ThenByDescending(o => o.IsTrending)
//                    .ToListAsync();

//                var response = offers.Select(o => MapToResponseDto(o)).ToList();
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message });
//            }
//        }

//        // GET: api/TblOffers/Featured
//        [HttpGet("Featured")]
//        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetFeaturedOffers()
//        {
//            try
//            {
//                var offers = await _context.TblOffers
//                    .Include(o => o.User)
//                    .Where(o => o.IsFeatured && o.Status && o.ValidUntil >= DateTime.Now)
//                    .OrderByDescending(o => o.CreatedDate)
//                    .ToListAsync();

//                var response = offers.Select(o => MapToResponseDto(o)).ToList();
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message });
//            }
//        }

//        // GET: api/TblOffers/Trending
//        [HttpGet("Trending")]
//        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetTrendingOffers()
//        {
//            try
//            {
//                var offers = await _context.TblOffers
//                    .Include(o => o.User)
//                    .Where(o => o.IsTrending && o.Status && o.ValidUntil >= DateTime.Now)
//                    .OrderByDescending(o => o.CreatedDate)
//                    .ToListAsync();

//                var response = offers.Select(o => MapToResponseDto(o)).ToList();
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message });
//            }
//        }

//        // POST: api/TblOffers
//        [HttpPost]
//        public async Task<ActionResult<OfferResponseDto>> CreateOffer([FromForm] CreateOfferDto dto)
//        {
//            try
//            {
//                // Validation
//                if (string.IsNullOrWhiteSpace(dto.Title) ||
//                    string.IsNullOrWhiteSpace(dto.Description) ||
//                    string.IsNullOrWhiteSpace(dto.Category) ||
//                    dto.UserId <= 0 ||
//                    dto.OriginalPrice <= 0 ||
//                    dto.DiscountValue <= 0)
//                {
//                    return BadRequest(new { success = false, message = "Please fill in all required fields with valid values." });
//                }

//                // Validate dates
//                if (dto.ValidFrom >= dto.ValidUntil)
//                {
//                    return BadRequest(new { success = false, message = "Valid Until date must be after Valid From date." });
//                }

//                var offer = new TblOffer
//                {
//                    Title = dto.Title.Trim(),
//                    Description = dto.Description.Trim(),
//                    DiscountType = dto.DiscountType,
//                    DiscountValue = dto.DiscountValue,
//                    OriginalPrice = dto.OriginalPrice,
//                    DiscountedPrice = dto.DiscountedPrice,
//                    Category = dto.Category.Trim(),
//                    ValidFrom = dto.ValidFrom,
//                    ValidUntil = dto.ValidUntil,
//                    IsFeatured = dto.IsFeatured,
//                    IsTrending = dto.IsTrending,
//                    Icon = dto.Icon?.Trim(),
//                    GradientClass = dto.GradientClass?.Trim(),
//                    AccentColor = dto.AccentColor?.Trim(),
//                    GamesIncluded = dto.GamesIncluded != null && dto.GamesIncluded.Any() ? JsonSerializer.Serialize(dto.GamesIncluded) : null,
//                    Terms = dto.Terms != null && dto.Terms.Any() ? JsonSerializer.Serialize(dto.Terms) : null,
//                    UserId = dto.UserId,
//                    Status = dto.Status,
//                    CreatedDate = DateTime.UtcNow,
//                    UpdatedDate = DateTime.UtcNow
//                };

//                // Handle image upload
//                if (dto.ImageFile != null && dto.ImageFile.Length > 0)
//                {
//                    var imageUrl = await SaveImageAsync(dto.ImageFile);
//                    if (imageUrl != null)
//                    {
//                        offer.ImageUrl = imageUrl;
//                    }
//                }

//                _context.TblOffers.Add(offer);
//                await _context.SaveChangesAsync();

//                var createdOffer = await _context.TblOffers
//                    .Include(o => o.User)
//                    .FirstOrDefaultAsync(o => o.OfferId == offer.OfferId);

//                return CreatedAtAction(nameof(GetOffer), new { id = offer.OfferId },
//                    new { success = true, message = "Offer created successfully.", data = MapToResponseDto(createdOffer!) });
//            }
//            catch (DbUpdateException ex)
//            {
//                return StatusCode(500, new { success = false, message = $"Database error: {ex.InnerException?.Message ?? ex.Message}" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
//            }
//        }

//        // PUT: api/TblOffers/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateOffer(int id, [FromForm] UpdateOfferDto dto)
//        {
//            try
//            {
//                if (id != dto.OfferId)
//                {
//                    return BadRequest(new { success = false, message = "Offer ID mismatch" });
//                }

//                var offer = await _context.TblOffers.FindAsync(id);
//                if (offer == null)
//                {
//                    return NotFound(new { success = false, message = "Offer not found" });
//                }

//                // Check if the user owns this offer
//                if (offer.UserId != dto.UserId)
//                {
//                    return Forbid("You can only update your own offers");
//                }

//                // Validation
//                if (string.IsNullOrWhiteSpace(dto.Title) ||
//                    string.IsNullOrWhiteSpace(dto.Description) ||
//                    string.IsNullOrWhiteSpace(dto.Category) ||
//                    dto.UserId <= 0 ||
//                    dto.OriginalPrice <= 0 ||
//                    dto.DiscountValue <= 0)
//                {
//                    return BadRequest(new { success = false, message = "Please fill in all required fields with valid values." });
//                }

//                // Validate dates
//                if (dto.ValidFrom >= dto.ValidUntil)
//                {
//                    return BadRequest(new { success = false, message = "Valid Until date must be after Valid From date." });
//                }

//                // Handle image upload
//                string imageUrl = offer.ImageUrl;
//                if (dto.ImageFile != null && dto.ImageFile.Length > 0)
//                {
//                    // Delete old image if exists
//                    if (!string.IsNullOrEmpty(offer.ImageUrl))
//                    {
//                        await DeleteImageAsync(offer.ImageUrl);
//                    }

//                    // Save new image
//                    var newImageUrl = await SaveImageAsync(dto.ImageFile);
//                    if (newImageUrl != null)
//                    {
//                        imageUrl = newImageUrl;
//                    }
//                }

//                // Update offer properties
//                offer.Title = dto.Title.Trim();
//                offer.Description = dto.Description.Trim();
//                offer.DiscountType = dto.DiscountType;
//                offer.DiscountValue = dto.DiscountValue;
//                offer.OriginalPrice = dto.OriginalPrice;
//                offer.DiscountedPrice = dto.DiscountedPrice;
//                offer.Category = dto.Category.Trim();
//                offer.ValidFrom = dto.ValidFrom;
//                offer.ValidUntil = dto.ValidUntil;
//                offer.IsFeatured = dto.IsFeatured;
//                offer.IsTrending = dto.IsTrending;
//                offer.Icon = dto.Icon?.Trim();
//                offer.GradientClass = dto.GradientClass?.Trim();
//                offer.AccentColor = dto.AccentColor?.Trim();
//                offer.ImageUrl = imageUrl;
//                offer.GamesIncluded = dto.GamesIncluded != null && dto.GamesIncluded.Any() ? JsonSerializer.Serialize(dto.GamesIncluded) : null;
//                offer.Terms = dto.Terms != null && dto.Terms.Any() ? JsonSerializer.Serialize(dto.Terms) : null;
//                offer.Status = dto.Status;
//                offer.UpdatedDate = DateTime.UtcNow;

//                _context.Entry(offer).State = EntityState.Modified;
//                await _context.SaveChangesAsync();

//                return Ok(new { success = true, message = "Offer updated successfully." });
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!OfferExists(id))
//                {
//                    return NotFound(new { success = false, message = "Offer not found" });
//                }
//                else
//                {
//                    throw;
//                }
//            }
//            catch (DbUpdateException ex)
//            {
//                return StatusCode(500, new { success = false, message = $"Database error: {ex.InnerException?.Message ?? ex.Message}" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
//            }
//        }

//        // DELETE: api/TblOffers/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteOffer(int id)
//        {
//            try
//            {
//                var offer = await _context.TblOffers.FindAsync(id);
//                if (offer == null)
//                {
//                    return NotFound(new { success = false, message = "Offer not found" });
//                }

//                // Delete associated image file
//                if (!string.IsNullOrEmpty(offer.ImageUrl))
//                {
//                    await DeleteImageAsync(offer.ImageUrl);
//                }

//                _context.TblOffers.Remove(offer);
//                await _context.SaveChangesAsync();

//                return Ok(new { success = true, message = "Offer deleted successfully." });
//            }
//            catch (DbUpdateException ex)
//            {
//                return StatusCode(500, new { success = false, message = $"Database error: {ex.InnerException?.Message ?? ex.Message}" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
//            }
//        }

//        // GET: api/TblOffers/Stats/5 (Back to int)
//        [HttpGet("Stats/{userId:int}")]
//        public async Task<ActionResult<object>> GetOfferStats(int userId)
//        {
//            try
//            {
//                var totalOffers = await _context.TblOffers.CountAsync(o => o.UserId == userId);
//                var activeOffers = await _context.TblOffers.CountAsync(o => o.UserId == userId && o.Status && o.ValidUntil >= DateTime.Now);
//                var featuredOffers = await _context.TblOffers.CountAsync(o => o.UserId == userId && o.IsFeatured);
//                var expiredOffers = await _context.TblOffers.CountAsync(o => o.UserId == userId && o.ValidUntil < DateTime.Now);

//                return Ok(new
//                {
//                    TotalOffers = totalOffers,
//                    ActiveOffers = activeOffers,
//                    FeaturedOffers = featuredOffers,
//                    ExpiredOffers = expiredOffers
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message });
//            }
//        }

//        private bool OfferExists(int id)
//        {
//            return _context.TblOffers.Any(e => e.OfferId == id);
//        }

//        private async Task<string?> SaveImageAsync(IFormFile imageFile)
//        {
//            try
//            {
//                // Check file extension
//                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
//                var fileExt = Path.GetExtension(imageFile.FileName).ToLower();
//                if (!allowedExtensions.Contains(fileExt))
//                    return null;

//                // Check file size (max 5MB)
//                const int maxFileSize = 5 * 1024 * 1024;
//                if (imageFile.Length > maxFileSize)
//                    return null;

//                // Ensure uploads folder exists
//                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
//                if (!Directory.Exists(uploadsPath))
//                    Directory.CreateDirectory(uploadsPath);

//                // Generate unique filename
//                var uniqueFileName = $"{Guid.NewGuid()}{fileExt}";
//                var savedPath = Path.Combine(uploadsPath, uniqueFileName);

//                // Save file
//                using (var fileStream = new FileStream(savedPath, FileMode.Create))
//                {
//                    await imageFile.CopyToAsync(fileStream);
//                }

//                return uniqueFileName; // Return just the filename, not the full path
//            }
//            catch (Exception ex)
//            {
//                // Log the exception if you have logging configured
//                Console.WriteLine($"Error saving image: {ex.Message}");
//                return null;
//            }
//        }

//        private async Task DeleteImageAsync(string imageUrl)
//        {
//            try
//            {
//                if (!string.IsNullOrEmpty(imageUrl))
//                {
//                    var imagePath = Path.Combine(_environment.WebRootPath, "uploads", imageUrl);
//                    if (System.IO.File.Exists(imagePath))
//                    {
//                        System.IO.File.Delete(imagePath);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                // Log the exception if you have logging configured
//                Console.WriteLine($"Error deleting image: {ex.Message}");
//            }
//        }

//        private OfferResponseDto MapToResponseDto(TblOffer offer)
//        {
//            var timeLeft = CalculateTimeLeft(offer.ValidUntil);
//            var gamesIncluded = !string.IsNullOrEmpty(offer.GamesIncluded)
//                ? JsonSerializer.Deserialize<List<string>>(offer.GamesIncluded) ?? new List<string>()
//                : new List<string>();
//            var terms = !string.IsNullOrEmpty(offer.Terms)
//                ? JsonSerializer.Deserialize<List<string>>(offer.Terms) ?? new List<string>()
//                : new List<string>();

//            return new OfferResponseDto
//            {
//                OfferId = offer.OfferId,
//                Title = offer.Title,
//                Description = offer.Description,
//                DiscountType = offer.DiscountType,
//                DiscountValue = offer.DiscountValue,
//                OriginalPrice = offer.OriginalPrice,
//                DiscountedPrice = offer.DiscountedPrice,
//                Category = offer.Category,
//                ValidFrom = offer.ValidFrom,
//                ValidUntil = offer.ValidUntil,
//                IsFeatured = offer.IsFeatured,
//                IsTrending = offer.IsTrending,
//                ImageUrl = offer.ImageUrl,
//                Icon = offer.Icon,
//                GradientClass = offer.GradientClass,
//                AccentColor = offer.AccentColor,
//                GamesIncluded = gamesIncluded,
//                Terms = terms,
//                UserId = offer.UserId,
//                UserName = offer.User?.UserName,
//                Status = offer.Status,
//                CreatedDate = offer.CreatedDate,
//                UpdatedDate = offer.UpdatedDate,
//                TimeLeft = timeLeft,
//                SavingsAmount = offer.OriginalPrice - offer.DiscountedPrice,
//                IsExpired = offer.ValidUntil < DateTime.Now
//            };
//        }

//        private string CalculateTimeLeft(DateTime validUntil)
//        {
//            var difference = validUntil - DateTime.Now;
//            if (difference.TotalDays > 0)
//            {
//                return $"{(int)difference.TotalDays} days left";
//            }
//            else if (difference.TotalHours > 0)
//            {
//                return $"{(int)difference.TotalHours} hours left";
//            }
//            else if (difference.TotalMinutes > 0)
//            {
//                return $"{(int)difference.TotalMinutes} minutes left";
//            }
//            else
//            {
//                return "Expired";
//            }
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameZoneManagementApi.Models;
using GameZoneManagementApi.DTOs;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace GameZoneManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblOffersController : ControllerBase
    {
        private readonly GamezoneDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TblOffersController(GamezoneDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/TblOffers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetOffers()
        {
            try
            {
                var offers = await _context.TblOffers
                    .Include(o => o.User)
                    .Where(o => o.Status && o.ValidUntil >= DateTime.Now)
                    .OrderByDescending(o => o.IsFeatured)
                    .ThenByDescending(o => o.IsTrending)
                    .ThenByDescending(o => o.CreatedDate)
                    .ToListAsync();

                var response = offers.Select(o => MapToResponseDto(o)).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // GET: api/TblOffers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfferResponseDto>> GetOffer(int id)
        {
            try
            {
                var offer = await _context.TblOffers
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.OfferId == id);

                if (offer == null)
                {
                    return NotFound(new { success = false, message = "Offer not found" });
                }

                return Ok(MapToResponseDto(offer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // GET: api/TblOffers/ByUser/5
        [HttpGet("ByUser/{userId:int}")]
        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetOffersByUser(int userId)
        {
            try
            {
                var offers = await _context.TblOffers
                    .Include(o => o.User)
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.CreatedDate)
                    .ToListAsync();

                var response = offers.Select(o => MapToResponseDto(o)).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // GET: api/TblOffers/ByCategory/weekend
        [HttpGet("ByCategory/{category}")]
        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetOffersByCategory(string category)
        {
            try
            {
                var offers = await _context.TblOffers
                    .Include(o => o.User)
                    .Where(o => o.Category.ToLower() == category.ToLower() &&
                               o.Status &&
                               o.ValidUntil >= DateTime.Now)
                    .OrderByDescending(o => o.IsFeatured)
                    .ThenByDescending(o => o.IsTrending)
                    .ToListAsync();

                var response = offers.Select(o => MapToResponseDto(o)).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // GET: api/TblOffers/Featured
        [HttpGet("Featured")]
        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetFeaturedOffers()
        {
            try
            {
                var offers = await _context.TblOffers
                    .Include(o => o.User)
                    .Where(o => o.IsFeatured && o.Status && o.ValidUntil >= DateTime.Now)
                    .OrderByDescending(o => o.CreatedDate)
                    .ToListAsync();

                var response = offers.Select(o => MapToResponseDto(o)).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // GET: api/TblOffers/Trending
        [HttpGet("Trending")]
        public async Task<ActionResult<IEnumerable<OfferResponseDto>>> GetTrendingOffers()
        {
            try
            {
                var offers = await _context.TblOffers
                    .Include(o => o.User)
                    .Where(o => o.IsTrending && o.Status && o.ValidUntil >= DateTime.Now)
                    .OrderByDescending(o => o.CreatedDate)
                    .ToListAsync();

                var response = offers.Select(o => MapToResponseDto(o)).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // POST: api/TblOffers
        [HttpPost]
        public async Task<ActionResult<OfferResponseDto>> CreateOffer([FromForm] CreateOfferDto dto)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(dto.Title) ||
                    string.IsNullOrWhiteSpace(dto.Description) ||
                    string.IsNullOrWhiteSpace(dto.Category) ||
                    dto.UserId <= 0 ||
                    dto.DiscountValue <= 0)
                {
                    return BadRequest(new { success = false, message = "Please fill in all required fields with valid values." });
                }

                // Validate dates
                if (dto.ValidFrom >= dto.ValidUntil)
                {
                    return BadRequest(new { success = false, message = "Valid Until date must be after Valid From date." });
                }

                // Validate discount value based on type
                if (dto.DiscountType == "percentage" && dto.DiscountValue > 100)
                {
                    return BadRequest(new { success = false, message = "Percentage discount cannot exceed 100%." });
                }

                var offer = new TblOffer
                {
                    Title = dto.Title.Trim(),
                    Description = dto.Description.Trim(),
                    DiscountType = dto.DiscountType,
                    DiscountValue = dto.DiscountValue,
                    Category = dto.Category.Trim(),
                    ValidFrom = dto.ValidFrom,
                    ValidUntil = dto.ValidUntil,
                    IsFeatured = dto.IsFeatured,
                    IsTrending = dto.IsTrending,
                    Icon = dto.Icon?.Trim(),
                    GradientClass = dto.GradientClass?.Trim(),
                    AccentColor = dto.AccentColor?.Trim(),
                    GamesIncluded = dto.GamesIncluded != null && dto.GamesIncluded.Any() ? JsonSerializer.Serialize(dto.GamesIncluded) : null,
                    Terms = dto.Terms != null && dto.Terms.Any() ? JsonSerializer.Serialize(dto.Terms) : null,
                    UserId = dto.UserId,
                    Status = dto.Status,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                // Handle image upload
                if (dto.ImageFile != null && dto.ImageFile.Length > 0)
                {
                    var imageUrl = await SaveImageAsync(dto.ImageFile);
                    if (imageUrl != null)
                    {
                        offer.ImageUrl = imageUrl;
                    }
                }

                _context.TblOffers.Add(offer);
                await _context.SaveChangesAsync();

                var createdOffer = await _context.TblOffers
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.OfferId == offer.OfferId);

                return CreatedAtAction(nameof(GetOffer), new { id = offer.OfferId },
                    new { success = true, message = "Offer created successfully.", data = MapToResponseDto(createdOffer!) });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { success = false, message = $"Database error: {ex.InnerException?.Message ?? ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        // PUT: api/TblOffers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOffer(int id, [FromForm] UpdateOfferDto dto)
        {
            try
            {
                if (id != dto.OfferId)
                {
                    return BadRequest(new { success = false, message = "Offer ID mismatch" });
                }

                var offer = await _context.TblOffers.FindAsync(id);
                if (offer == null)
                {
                    return NotFound(new { success = false, message = "Offer not found" });
                }

                // Check if the user owns this offer
                if (offer.UserId != dto.UserId)
                {
                    return Forbid("You can only update your own offers");
                }

                // Validation
                if (string.IsNullOrWhiteSpace(dto.Title) ||
                    string.IsNullOrWhiteSpace(dto.Description) ||
                    string.IsNullOrWhiteSpace(dto.Category) ||
                    dto.UserId <= 0 ||
                    dto.DiscountValue <= 0)
                {
                    return BadRequest(new { success = false, message = "Please fill in all required fields with valid values." });
                }

                // Validate dates
                if (dto.ValidFrom >= dto.ValidUntil)
                {
                    return BadRequest(new { success = false, message = "Valid Until date must be after Valid From date." });
                }

                // Validate discount value based on type
                if (dto.DiscountType == "percentage" && dto.DiscountValue > 100)
                {
                    return BadRequest(new { success = false, message = "Percentage discount cannot exceed 100%." });
                }

                // Handle image upload
                string imageUrl = offer.ImageUrl;
                if (dto.ImageFile != null && dto.ImageFile.Length > 0)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(offer.ImageUrl))
                    {
                        await DeleteImageAsync(offer.ImageUrl);
                    }

                    // Save new image
                    var newImageUrl = await SaveImageAsync(dto.ImageFile);
                    if (newImageUrl != null)
                    {
                        imageUrl = newImageUrl;
                    }
                }

                // Update offer properties
                offer.Title = dto.Title.Trim();
                offer.Description = dto.Description.Trim();
                offer.DiscountType = dto.DiscountType;
                offer.DiscountValue = dto.DiscountValue;
                offer.Category = dto.Category.Trim();
                offer.ValidFrom = dto.ValidFrom;
                offer.ValidUntil = dto.ValidUntil;
                offer.IsFeatured = dto.IsFeatured;
                offer.IsTrending = dto.IsTrending;
                offer.Icon = dto.Icon?.Trim();
                offer.GradientClass = dto.GradientClass?.Trim();
                offer.AccentColor = dto.AccentColor?.Trim();
                offer.ImageUrl = imageUrl;
                offer.GamesIncluded = dto.GamesIncluded != null && dto.GamesIncluded.Any() ? JsonSerializer.Serialize(dto.GamesIncluded) : null;
                offer.Terms = dto.Terms != null && dto.Terms.Any() ? JsonSerializer.Serialize(dto.Terms) : null;
                offer.Status = dto.Status;
                offer.UpdatedDate = DateTime.UtcNow;

                _context.Entry(offer).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Offer updated successfully." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfferExists(id))
                {
                    return NotFound(new { success = false, message = "Offer not found" });
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { success = false, message = $"Database error: {ex.InnerException?.Message ?? ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        // DELETE: api/TblOffers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            try
            {
                var offer = await _context.TblOffers.FindAsync(id);
                if (offer == null)
                {
                    return NotFound(new { success = false, message = "Offer not found" });
                }

                // Delete associated image file
                if (!string.IsNullOrEmpty(offer.ImageUrl))
                {
                    await DeleteImageAsync(offer.ImageUrl);
                }

                _context.TblOffers.Remove(offer);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Offer deleted successfully." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { success = false, message = $"Database error: {ex.InnerException?.Message ?? ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        // GET: api/TblOffers/Stats/5
        [HttpGet("Stats/{userId:int}")]
        public async Task<ActionResult<object>> GetOfferStats(int userId)
        {
            try
            {
                var totalOffers = await _context.TblOffers.CountAsync(o => o.UserId == userId);
                var activeOffers = await _context.TblOffers.CountAsync(o => o.UserId == userId && o.Status && o.ValidUntil >= DateTime.Now);
                var featuredOffers = await _context.TblOffers.CountAsync(o => o.UserId == userId && o.IsFeatured);
                var expiredOffers = await _context.TblOffers.CountAsync(o => o.UserId == userId && o.ValidUntil < DateTime.Now);

                return Ok(new
                {
                    TotalOffers = totalOffers,
                    ActiveOffers = activeOffers,
                    FeaturedOffers = featuredOffers,
                    ExpiredOffers = expiredOffers
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        private bool OfferExists(int id)
        {
            return _context.TblOffers.Any(e => e.OfferId == id);
        }

        private async Task<string?> SaveImageAsync(IFormFile imageFile)
        {
            try
            {
                // Check file extension
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
                var fileExt = Path.GetExtension(imageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExt))
                    return null;

                // Check file size (max 5MB)
                const int maxFileSize = 5 * 1024 * 1024;
                if (imageFile.Length > maxFileSize)
                    return null;

                // Ensure uploads folder exists
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                // Generate unique filename
                var uniqueFileName = $"{Guid.NewGuid()}{fileExt}";
                var savedPath = Path.Combine(uploadsPath, uniqueFileName);

                // Save file
                using (var fileStream = new FileStream(savedPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                return uniqueFileName; // Return just the filename, not the full path
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging configured
                Console.WriteLine($"Error saving image: {ex.Message}");
                return null;
            }
        }

        private async Task DeleteImageAsync(string imageUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    var imagePath = Path.Combine(_environment.WebRootPath, "uploads", imageUrl);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging configured
                Console.WriteLine($"Error deleting image: {ex.Message}");
            }
        }

        private OfferResponseDto MapToResponseDto(TblOffer offer)
        {
            var timeLeft = CalculateTimeLeft(offer.ValidUntil);
            var gamesIncluded = !string.IsNullOrEmpty(offer.GamesIncluded)
                ? JsonSerializer.Deserialize<List<string>>(offer.GamesIncluded) ?? new List<string>()
                : new List<string>();
            var terms = !string.IsNullOrEmpty(offer.Terms)
                ? JsonSerializer.Deserialize<List<string>>(offer.Terms) ?? new List<string>()
                : new List<string>();

            // Create discount display string
            var discountDisplay = offer.DiscountType == "percentage"
                ? $"{offer.DiscountValue}% OFF"
                : $"₹{offer.DiscountValue} OFF";

            return new OfferResponseDto
            {
                OfferId = offer.OfferId,
                Title = offer.Title,
                Description = offer.Description,
                DiscountType = offer.DiscountType,
                DiscountValue = offer.DiscountValue,
                Category = offer.Category,
                ValidFrom = offer.ValidFrom,
                ValidUntil = offer.ValidUntil,
                IsFeatured = offer.IsFeatured,
                IsTrending = offer.IsTrending,
                ImageUrl = offer.ImageUrl,
                Icon = offer.Icon,
                GradientClass = offer.GradientClass,
                AccentColor = offer.AccentColor,
                GamesIncluded = gamesIncluded,
                Terms = terms,
                UserId = offer.UserId,
                UserName = offer.User?.UserName,
                Status = offer.Status,
                CreatedDate = offer.CreatedDate,
                UpdatedDate = offer.UpdatedDate,
                TimeLeft = timeLeft,
                DiscountDisplay = discountDisplay,
                IsExpired = offer.ValidUntil < DateTime.Now
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
}
