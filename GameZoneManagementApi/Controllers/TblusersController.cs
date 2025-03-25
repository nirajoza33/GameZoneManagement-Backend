using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameZoneManagementApi.Models;
using GameZoneManagementApi.DTOs;
using System.Net.Mail;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameZoneManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblusersController : ControllerBase
    {
        private readonly GamezoneDbContext _context;
        private readonly IConfiguration _configuration;

        public TblusersController(GamezoneDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Tblusers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tblusers>>> GetTblusers()
        {
            string secretKey = "Game-Zone-Management"; // Replace with your key
            string formattedKey = FormatKeyTo256Bit(secretKey);

            Console.WriteLine("256-bit Key: " + formattedKey);
            return await _context.Tblusers.ToListAsync();
        }


        static string FormatKeyTo256Bit(string key)
        {
            const int requiredLength = 32; // 256-bit = 32 bytes
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            if (keyBytes.Length == requiredLength)
                return Convert.ToBase64String(keyBytes);

            byte[] resizedKey = new byte[requiredLength];
            Array.Copy(keyBytes, resizedKey, Math.Min(keyBytes.Length, requiredLength));

            return Convert.ToBase64String(resizedKey);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserDto registerUserDto)
        {
            if (_context.Tblusers.Any(u => u.Email == registerUserDto.Email))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Email Already Exists"
                });

            }

            var user = new Tblusers
            {
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password),
                phone = registerUserDto.phone,
                Bio = registerUserDto.Bio,
                RoleId = registerUserDto.RoleId,


            };

            _context.Tblusers.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Registered Successfully.",
                user = new
                {
                    user.UserId,
                    user.UserName,
                    user.Email,
                    user.phone,
                    user.Bio,
                    user.RoleId
                }
            });
        }

        [HttpPost("send-otp")]
        public async Task<ActionResult> SendOtp([FromBody] SendOtpDto sendOtpDto)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            HttpContext.Session.SetString("otp", otp);
            HttpContext.Session.SetString("otpEmail", sendOtpDto.Email);

            bool emailSent = await SendEmail(sendOtpDto.Email, otp);

            if (!emailSent)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "failed to send OTP."
                });
            }
            else
            {
                return Ok(new
                {
                    success = true,
                    message = "OTP sent Successfully."
                });
            }
        }

        // email sending method
        private async Task<bool> SendEmail(string email, string otp)
        {
            try
            {
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "OtpTemplate.html");
                string emailBody = await System.IO.File.ReadAllTextAsync(templatePath);

                emailBody = emailBody.Replace("{{OTP}}", otp);

                // App Password :- bmox lxai tdxv pgct
                using var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("ozaniraj19@gmail.com", "bmox lxai tdxv pgct"),
                    EnableSsl = true,
                };

                var mailMsg = new MailMessage
                {
                    From = new MailAddress("ozaniraj19@gmail.com"),
                    Subject = "Your OTP code for Registration.",
                    Body = emailBody,
                    IsBodyHtml = true,

                };

                mailMsg.To.Add(email);
                await smtp.SendMailAsync(mailMsg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // verify otp functionality
        [HttpPost("verify-otp")]
        public async Task<ActionResult> VerifyOtp([FromBody] VerifyOtpDto verifyOtpDto)
        {
            var sessionOtp = HttpContext.Session.GetString("otp");
            var sessionEmail = HttpContext.Session.GetString("otpEmail");

            if (sessionOtp == null || sessionEmail == null || sessionOtp != verifyOtpDto.Otp || sessionEmail != verifyOtpDto.Email)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Or Expired OTP."
                });
            }

            HttpContext.Session.Remove("otp");
            HttpContext.Session.Remove("otpEmail");

            return Ok(new
            {
                success = true,
                message = "OTP Verified Successfully."
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult> login([FromBody] LoginUserDto loginUserDto)
        {
            var user = _context.Tblusers.Include(u => u.Tblrole).Where(u => u.Email == loginUserDto.Email).FirstOrDefault();
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUserDto.Password, user.Password))
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Invalid Email and Password"
                });
            }

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                success = true,
                message = "Login Successfull.",
                token = token,
                roleId = user.RoleId,
                redirectUrl = GetRedirectUrl(user.RoleId)
            });
        }

        private string GenerateJwtToken(Tblusers user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim("UserId", user.UserId.ToString()),
            new Claim("Tblrole", user.Tblrole.RoleName.ToString()),
            new Claim("RoleId", user.RoleId.ToString()),
        };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GetRedirectUrl(int roleId)
        {
            return roleId switch
            {
                1 => "/admin/dashboard",
                2 => "/provider/dashboard",
                3 => "/client/dashboard",
                _ => "/"
            };
        }



        //// GET: api/Tblusers/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Tblusers>> GetTblusers(int id)
        //{
        //    var tblusers = await _context.Tblusers.FindAsync(id);

        //    if (tblusers == null)
        //    {
        //        return NotFound();
        //    }

        //    return tblusers;
        //}

        //// PUT: api/Tblusers/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTblusers(int id, Tblusers tblusers)
        //{
        //    if (id != tblusers.UserId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(tblusers).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TblusersExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Tblusers
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Tblusers>> PostTblusers(Tblusers tblusers)
        //{
        //    _context.Tblusers.Add(tblusers);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTblusers", new { id = tblusers.UserId }, tblusers);
        //}

        //// DELETE: api/Tblusers/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTblusers(int id)
        //{
        //    var tblusers = await _context.Tblusers.FindAsync(id);
        //    if (tblusers == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Tblusers.Remove(tblusers);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool TblusersExists(int id)
        {
            return _context.Tblusers.Any(e => e.UserId == id);
        }
    }
}
