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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GameZoneManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblusersController : ControllerBase
    {
        private readonly GamezoneDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public TblusersController(GamezoneDbContext context, IConfiguration configuration, IWebHostEnvironment env)
        {
            _context = context;
            _configuration = configuration;
            _env = env;
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


        //[HttpPost("register")]
        //public async Task<ActionResult> Register(RegisterUserDto registerUserDto)
        //{
        //    try
        //    {
        //        // 1. CAPTCHA Verification
        //        if (!_env.IsDevelopment()) // Only verify in production
        //        {
        //            var captchaValid = await VerifyCaptchaAsync(registerUserDto.CaptchaToken);
        //            if (!captchaValid)
        //            {
        //                return BadRequest(new
        //                {
        //                    success = false,
        //                    message = "CAPTCHA validation failed."
        //                });
        //            }
        //        }


        //        // 2. Email Check
        //        if (_context.Tblusers.Any(u => u.Email == registerUserDto.Email))
        //        {
        //            return BadRequest(new
        //            {
        //                success = false,
        //                message = "Email Already Exists"
        //            });

        //        }

        //        var user = new Tblusers
        //        {
        //            UserName = registerUserDto.UserName,
        //            Email = registerUserDto.Email,
        //            Password = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password),
        //            phone = registerUserDto.phone,
        //            Bio = registerUserDto.Bio,
        //            RoleId = registerUserDto.RoleId,
        //            Status = registerUserDto.Status


        //        };

        //        _context.Tblusers.Add(user);
        //        await _context.SaveChangesAsync();

        //        return Ok(new
        //        {
        //            success = true,
        //            message = "Registered Successfully.",
        //            user = new
        //            {
        //                user.UserId,
        //                user.UserName,
        //                user.Email,
        //                user.phone,
        //                user.Bio,
        //                user.RoleId,
        //                user.Status
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { success = false, message = ex.Message });
        //    }
        //}


        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserDto registerUserDto)
        {
            try
            {
                // 1. CAPTCHA Verification
                var captchaVerified = HttpContext.Session.GetString("CaptchaVerified");
                var captchaVerifiedTimeStr = HttpContext.Session.GetString("CaptchaVerifiedTime");

                if (captchaVerified != "true" || string.IsNullOrEmpty(captchaVerifiedTimeStr))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "CAPTCHA verification required."
                    });
                }

                // Check if CAPTCHA verification has expired (10 minutes)
                if (DateTime.TryParse(captchaVerifiedTimeStr, out var verifiedTime))
                {
                    if ((DateTime.UtcNow - verifiedTime).TotalMinutes > 10)
                    {
                        HttpContext.Session.Remove("CaptchaVerified");
                        HttpContext.Session.Remove("CaptchaVerifiedTime");
                        return BadRequest(new
                        {
                            success = false,
                            message = "CAPTCHA verification has expired. Please verify CAPTCHA again."
                        });
                    }
                }

                // 2. Email Check
                if (_context.Tblusers.Any(u => u.Email == registerUserDto.Email))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Email Already Exists"
                    });
                }

                // 3. Username Check
                if (_context.Tblusers.Any(u => u.UserName == registerUserDto.UserName))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Username Already Exists"
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
                    Status = registerUserDto.Status ?? "active"
                };

                _context.Tblusers.Add(user);
                await _context.SaveChangesAsync();

                // Clear CAPTCHA verification after successful registration
                HttpContext.Session.Remove("CaptchaVerified");
                HttpContext.Session.Remove("CaptchaVerifiedTime");

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
                        user.RoleId,
                        user.Status
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }



        private async Task<bool> VerifyCaptchaAsync(string captchaToken)
        {
            var secretKey = "6LfTQ04rAAAAAAmHQ3cEA8WgVlPnTvifpliP8dXZ"; // 🔐 Replace with your actual secret key
            using var client = new HttpClient();
            var response = await client.PostAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaToken}",
                null
            );

            if (!response.IsSuccessStatusCode)
                return false;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var captchaResult = System.Text.Json.JsonSerializer.Deserialize<RecaptchaResponse>(jsonResponse);
            //return captchaResult.success && captchaResult.score >= 0.5;
            if (captchaResult.success && captchaResult.score >= 0.5)
            {
                return true;
            }

            return false;


        }

        public class RecaptchaResponse
        {
            public bool success { get; set; }
            public float score { get; set; }
            public string action { get; set; }
            public DateTime challenge_ts { get; set; }
            public string hostname { get; set; }
            public List<string> error_codes { get; set; }
        }


        [HttpPost("send-otp")]
        public async Task<ActionResult> SendOtp([FromBody] SendOtpDto sendOtpDto)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            HttpContext.Session.SetString("otp", otp);
            HttpContext.Session.SetString("otpEmail", sendOtpDto.Email);

            Console.WriteLine($"OTP Stored: {otp} for Email: {sendOtpDto.Email}"); // Debugging log

            bool emailSent = await SendEmail(sendOtpDto.Email, otp);

            if (!emailSent)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Failed to send OTP."
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

            Console.WriteLine($"OTP Retrieved: {sessionOtp} for Email: {sessionEmail}"); // Debugging log


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

        //private string GenerateJwtToken(Tblusers user)
        //{
        //    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //        new Claim("UserId", user.UserId.ToString()),
        //        new Claim(ClaimTypes.Role, user.Tblrole.RoleName.ToString()),
        //        new Claim("RoleId", user.RoleId.ToString()),
        //        new Claim("UserName", user.UserName),
        //        new Claim("Phone", user.phone),
        //        new Claim("Bio", user.Bio),
        //    };

        //    var token = new JwtSecurityToken(
        //        _configuration["Jwt:Issuer"],
        //        _configuration["Jwt:Issuer"],
        //        claims,
        //        expires: DateTime.UtcNow.AddHours(2),
        //        signingCredentials: new SigningCredentials(
        //            new SymmetricSecurityKey(key),
        //            SecurityAlgorithms.HmacSha256)
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        private string GenerateJwtToken(Tblusers user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? ""),
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Tblrole?.RoleName ?? "User"),
                new Claim("RoleId", user.RoleId.ToString()),
                new Claim("UserName", user.UserName ?? ""),
                new Claim("Phone", user.phone ?? ""),
                new Claim("Bio", user.Bio ?? ""),
                new Claim("Status", user.Status ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
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

        private bool TblusersExists(int id)
        {
            return _context.Tblusers.Any(e => e.UserId == id);
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

        // PUT: api/Tblusers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblusers(int id, UpdateUserDto updateUserDto)
        {
            var existingUser = await _context.Tblusers.FindAsync(id);
            if (id != existingUser.UserId)
            {
                return BadRequest();
            }

            _context.Entry(existingUser).State = EntityState.Modified;

            try
            {
                existingUser.Bio = updateUserDto.Bio;
                existingUser.phone = updateUserDto.Phone;
                existingUser.Email = updateUserDto.Email;
                existingUser.UserName = updateUserDto.UserName;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblusersExists(id))
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

        //// POST: api/Tblusers
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Tblusers>> PostTblusers(Tblusers tblusers)
        //{
        //    _context.Tblusers.Add(tblusers);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTblusers", new { id = tblusers.UserId }, tblusers);
        //}

        // DELETE: api/Tblusers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblusers(int id)
        {
            var tblusers = await _context.Tblusers.FindAsync(id);
            if (tblusers == null)
            {
                return NotFound();
            }

            _context.Tblusers.Remove(tblusers);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/TblUsers/{id}/suspend
        [HttpPatch("{id}/suspend")]
        public async Task<IActionResult> SuspendUser(int id)
        {
            var user = await _context.Tblusers.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            user.Status = "suspended"; // or "inactive", based on your schema
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "User suspended successfully" });
        }

        // PATCH: api/TblUsers/{id}/activate
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var user = await _context.Tblusers.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            user.Status = "active";
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "User activated successfully" });
        }



        //private readonly PasswordHasher<Tblusers> _passwordHasher = new PasswordHasher<Tblusers>();

        //[HttpPost("forgot-password")]
        //[AllowAnonymous]
        //public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        //{
        //    var user = await _context.Tblusers.FirstOrDefaultAsync(u => u.Email == dto.Email);
        //    if (user == null)
        //        return NotFound("User not found");

        //    // Generate token & store in session
        //    var token = Guid.NewGuid().ToString();
        //    HttpContext.Session.SetString("ResetToken", token);
        //    HttpContext.Session.SetString("ResetEmail", dto.Email);
        //    HttpContext.Session.SetString("TokenCreatedAt", DateTime.UtcNow.ToString());

        //    var resetLink = $"http://localhost:3000/reset-password?token={token}";

        //    // Simulate sending email
        //    Console.WriteLine($"Reset password link: {resetLink}");

        //    return Ok(new { message = "Reset link has been sent." });
        //}

        //[HttpPost("reset-password")]
        //[AllowAnonymous]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        //{
        //    var storedToken = HttpContext.Session.GetString("ResetToken");
        //    var storedEmail = HttpContext.Session.GetString("ResetEmail");
        //    var tokenTimeStr = HttpContext.Session.GetString("TokenCreatedAt");

        //    if (storedToken == null || storedEmail == null || tokenTimeStr == null)
        //        return BadRequest("Session expired or invalid.");

        //    if (storedToken != dto.Token || storedEmail != dto.Email)
        //        return BadRequest("Invalid token or email.");

        //    if (!DateTime.TryParse(tokenTimeStr, out var tokenTime) || tokenTime.AddMinutes(30) < DateTime.UtcNow)
        //        return BadRequest("Reset token has expired.");

        //    var user = await _context.Tblusers.FirstOrDefaultAsync(u => u.Email == dto.Email);
        //    if (user == null)
        //        return NotFound("User not found.");

        //    user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        //    _context.Tblusers.Update(user);
        //    await _context.SaveChangesAsync();

        //    // Clear the session after successful password reset
        //    HttpContext.Session.Remove("ResetToken");
        //    HttpContext.Session.Remove("ResetEmail");
        //    HttpContext.Session.Remove("TokenCreatedAt");

        //    return Ok(new { success = true, message = "Password has been reset successfully." });
        //}


        //[HttpPost("change-password")]
        //public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId"); // Assumes you store this on login
        //    if (userId == null)
        //        return Unauthorized();

        //    var user = await _context.Tblusers.FindAsync(userId.Value);
        //    if (user == null)
        //        return Unauthorized();

        //    var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.OldPassword);
        //    if (result != PasswordVerificationResult.Success)
        //        return BadRequest("Old password is incorrect.");

        //    user.Password = _passwordHasher.HashPassword(user, dto.NewPassword);
        //    await _context.SaveChangesAsync();

        //    return Ok(new { message = "Password changed successfully." });
        //}


        [HttpPost("send-forgot-otp")]
        public IActionResult SendForgotOtp([FromForm] string toEmail)
        {
            var fromEmail = _configuration["Smtp:Email"];
            var password = _configuration["Smtp:Password"];
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPort = int.Parse(_configuration["Smtp:Port"]);

            var otp = new Random().Next(100000, 999999).ToString();

            var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = "Your OTP Code",
                Body = $"Your OTP code is: {otp}",
                IsBodyHtml = false
            };

            using (var smtp = new SmtpClient(smtpHost, smtpPort))
            {
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(message);

                    // You can store OTP in session or DB here
                    HttpContext.Session.SetString("OTP", otp);
                    HttpContext.Session.SetString("ResetEmail", toEmail);


                    return Ok("OTP sent successfully");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error sending email: {ex.Message}");
                }
            }
        }


        [HttpPost("verify-otp-and-reset-password")]
        public async Task<IActionResult> VerifyOtpAndResetPassword([FromBody] ResetPasswordDto resetDto)
        {
            var sessionOtp = HttpContext.Session.GetString("OTP");
            var resetEmail = HttpContext.Session.GetString("ResetEmail");

            if (string.IsNullOrEmpty(sessionOtp) || string.IsNullOrEmpty(resetEmail))
            {
                return BadRequest(new { success = false, message = "OTP session expired or email not found." });
            }

            if (resetDto.Otp != sessionOtp)
            {
                return BadRequest(new { success = false, message = "Invalid OTP" });
            }

            if (resetDto.NewPassword != resetDto.ConfirmPassword)
            {
                return BadRequest(new { success = false, message = "Passwords do not match." });
            }

            var user = await _context.Tblusers.FirstOrDefaultAsync(u => u.Email == resetEmail);

            if (user == null)   
            {
                return BadRequest(new { success = false, message = "User not found." });
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(resetDto.NewPassword);
            await _context.SaveChangesAsync();

            // Clear OTP session after successful reset
            HttpContext.Session.Remove("OTP");
            HttpContext.Session.Remove("ResetEmail");

            return Ok(new { success = true, message = "Password reset successfully." });
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            // Find the user by UserId
            var user = await _context.Tblusers.FirstOrDefaultAsync(u => u.UserId == changePasswordDto.UserId);
            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "User not found"
                });
            }

            // Check if the old password is correct
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, user.Password))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Old password is incorrect"
                });
            }

            // Hash and update the new password
            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            _context.Tblusers.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Password changed successfully"
            });
        }




            


        // POST: Admin/create-owner
        [HttpPost("Admin/create-owner")]
        //[Authorize(Roles = "admin")] // Only Admin can create owner accounts
        public async Task<ActionResult> CreateOwner([FromBody] CreateOwnerDto ownerDto)
        {
            try
            {
                if (ownerDto == null || string.IsNullOrWhiteSpace(ownerDto.Email))
                {
                    return BadRequest(new { success = false, message = "Invalid owner data." });
                }

                // Check if email already exists
                if (await _context.Tblusers.AnyAsync(u => u.Email == ownerDto.Email))
                {
                    return BadRequest(new { success = false, message = "Email already exists." });
                }

                // Step 1: Generate a random password
                string rawPassword = GenerateRandomPassword();

                // Step 2: Send email with the raw password
                bool emailSent = await SendOwnerCredentialsEmail(ownerDto.Email, rawPassword);
                if (!emailSent)
                {
                    return StatusCode(500, new { success = false, message = "Failed to send credentials email." });
                }

                // Step 3: Hash the password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword);

                // Step 4: Create user entity and save
                var user = new Tblusers
                {
                    UserName = ownerDto.UserName,
                    Email = ownerDto.Email,
                    phone = ownerDto.Phone,
                    RoleId = 2,  
                    Password = hashedPassword,
                    Status = ownerDto.Status
                };

                _context.Tblusers.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Owner created successfully and credentials sent via email.",
                    password = rawPassword, // 👈 Send back plain password   
                    user = new
                    {
                        user.UserId,
                        user.UserName,
                        user.Email,
                        user.phone,
                        user.RoleId,
                        user.Status
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: Admin/get-owners
        [HttpGet("Admin/get-owners")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllOwners()
        {
            var owners = await _context.Tblusers
                .Where(u => u.RoleId == 2)  // RoleId 3 for owners
                .Select(u => new {
                    u.UserId,
                    u.UserName,
                    u.Email,
                    u.phone,
                    u.Status
                }).ToListAsync();

            return Ok(new { success = true, data = owners });
        }

        // DELETE: Admin/delete-owner/{id}
        [HttpDelete("Admin/delete-owner/{id}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteOwner(int id)
        {
            var user = await _context.Tblusers.FindAsync(id);
            if (user == null || user.RoleId != 2) // RoleId 3 for owner
                return NotFound(new { success = false, message = "Owner not found." });

            _context.Tblusers.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Owner deleted successfully." });
        }

        // Helper Methods
        private string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async Task<bool> SendOwnerCredentialsEmail(string email, string password)
        {
            try
            {
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "OwnerCredentialTemplate.html");
                string emailBody = await System.IO.File.ReadAllTextAsync(templatePath);

                // Replace placeholders
                emailBody = emailBody.Replace("{{Email}}", email)
                                   .Replace("{{Password}}", password);

                using var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("ozaniraj19@gmail.com", "bmox lxai tdxv pgct"), // Consider storing securely
                    EnableSsl = true,
                };

                var mail = new MailMessage
                {
                    From = new MailAddress("ozaniraj19@gmail.com"),
                    Subject = "Your Owner Account Credentials",
                    Body = emailBody,
                    IsBodyHtml = true
                };

                mail.To.Add(email);
                await smtp.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }





        // GET: api/Analytics/Dashboard/{userId}
        [HttpGet("Dashboard/{userId}")]
        public async Task<ActionResult<object>> GetDashboardData(int userId)
        {
            try
            {
                // Get all games owned by this user
                var userGames = await _context.Games
                    .Where(g => g.UserId == userId)
                    .ToListAsync();

                // Calculate total revenue from payments for user's games
                var totalRevenue = await _context.TblPayments
                    .Where(p => _context.Games.Any(g => g.GameId == p.GameId && g.UserId == userId) &&
                               p.PaymentStatus == PaymentStatus.Completed)
                    .SumAsync(p => p.Amount);

                // Count unique users who booked the user's games
                var totalUsers = await _context.TblBookings
                    .Where(b => _context.Games.Any(g => g.GameId == b.GameId && g.UserId == userId))
                    .Select(b => b.UserId)
                    .Distinct()
                    .CountAsync();

                // Count active games
                var activeGames = userGames.Count(g => g.Status);

                // Calculate conversion rate (revenue per user)
                var conversionRate = totalUsers > 0 ? Math.Round((double)totalRevenue / totalUsers / 1000, 1) : 0;

                // Calculate monthly growth (comparing current month to previous month)
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;
                var previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;
                var previousYear = currentMonth == 1 ? currentYear - 1 : currentYear;

                var currentMonthRevenue = await _context.TblPayments
                    .Where(p => _context.Games.Any(g => g.GameId == p.GameId && g.UserId == userId) &&
                               p.PaymentStatus == PaymentStatus.Completed &&
                               p.PaymentDate.Month == currentMonth &&
                               p.PaymentDate.Year == currentYear)
                    .SumAsync(p => p.Amount);

                var previousMonthRevenue = await _context.TblPayments
                    .Where(p => _context.Games.Any(g => g.GameId == p.GameId && g.UserId == userId) &&
                               p.PaymentStatus == PaymentStatus.Completed &&
                               p.PaymentDate.Month == previousMonth &&
                               p.PaymentDate.Year == previousYear)
                    .SumAsync(p => p.Amount);

                var revenueGrowth = previousMonthRevenue > 0
                    ? Math.Round(((decimal)currentMonthRevenue - previousMonthRevenue) / previousMonthRevenue * 100, 1)
                    : 0;

                return Ok(new
                {
                    totalRevenue,
                    totalUsers,
                    activeGames,
                    conversionRate,
                    totalGames = userGames.Count,
                    revenueGrowth
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Analytics/GameRevenue/{userId}
        [HttpGet("GameRevenue/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetGameRevenue(int userId)
        {
            try
            {
                var gameRevenue = await (
                    from game in _context.Games
                    join payment in _context.TblPayments on game.GameId equals payment.GameId into payments
                    from p in payments.DefaultIfEmpty()
                    where game.UserId == userId
                    group p by new { game.GameId, game.Title, game.Category.CategoryName } into grp
                    select new
                    {
                        gameId = grp.Key.GameId,
                        name = grp.Key.Title,
                        category = grp.Key.CategoryName ?? "Unknown",
                        revenue = grp.Sum(p => p != null && p.PaymentStatus == PaymentStatus.Completed ? p.Amount : 0),
                        players = grp.Count(p => p != null),
                        downloads = grp.Count(p => p != null) + new Random().Next(10, 100) // Simulated downloads
                    }
                ).ToListAsync();

                return Ok(gameRevenue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Analytics/UserEngagement/{userId}
        [HttpGet("UserEngagement/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserEngagement(int userId)
        {
            try
            {
                // Get bookings for user's games to analyze engagement by time of day
                var bookings = await _context.TblBookings
                    .Include(b => b.Game)
                    .Where(b => b.Game.UserId == userId)
                    .ToListAsync();

                // Group bookings by time slots (4-hour intervals)
                var timeSlots = new[] { "00:00", "04:00", "08:00", "12:00", "16:00", "20:00" };
                var engagementData = new List<object>();

                foreach (var slot in timeSlots)
                {
                    var hour = int.Parse(slot.Split(':')[0]);
                    var slotBookings = bookings.Where(b =>
                        b.StartTime.Hours >= hour &&
                        b.StartTime.Hours < (hour + 4)).ToList();

                    engagementData.Add(new
                    {
                        time = slot,
                        active = slotBookings.Count,
                        new_users = slotBookings.Select(b => b.UserId).Distinct().Count() / 3 // Approximation of new users
                    });
                }

                return Ok(engagementData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Analytics/RevenueBreakdown/{userId}
        [HttpGet("RevenueBreakdown/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetRevenueBreakdown(int userId)
        {
            try
            {
                // Get all games by this user
                var userGames = await _context.Games
                    .Include(g => g.Category)
                    .Where(g => g.UserId == userId)
                    .ToListAsync();

                // Get all payments for these games
                var gameIds = userGames.Select(g => g.GameId).ToList();
                var payments = await _context.TblPayments
                    .Where(p => gameIds.Contains(p.GameId) && p.PaymentStatus == PaymentStatus.Completed)
                    .ToListAsync();

                // Group by category
                var categoryGroups = userGames
                    .GroupBy(g => g.Category?.CategoryName ?? "Unknown")
                    .Select(g => new
                    {
                        name = g.Key,
                        games = g.ToList()
                    })
                    .ToList();

                // Calculate revenue by category
                var breakdown = categoryGroups.Select(cg => {
                    var categoryGameIds = cg.games.Select(g => g.GameId).ToList();
                    var categoryRevenue = payments
                        .Where(p => categoryGameIds.Contains(p.GameId))
                        .Sum(p => p.Amount);

                    return new
                    {
                        name = cg.name,
                        amount = categoryRevenue
                    };
                }).ToList();

                var totalRevenue = breakdown.Sum(b => b.amount);

                var result = breakdown.Select(b => new
                {
                    name = b.name,
                    value = totalRevenue > 0 ? Math.Round((b.amount / totalRevenue) * 100, 1) : 0,
                    amount = b.amount
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Analytics/GeographicData
        [HttpGet("GeographicData")]
        public async Task<ActionResult<IEnumerable<object>>> GetGeographicData()
        {
            try
            {
                // Since we don't have actual geographic data in the models,
                // we'll create simulated data based on user distribution
                var totalUsers = await _context.Tblusers.CountAsync();

                // Simulated geographic distribution
                var countries = new[]
                {
                    new { country = "India", users = (int)(totalUsers * 0.4), revenue = 45000m },
                    new { country = "USA", users = (int)(totalUsers * 0.25), revenue = 32000m },
                    new { country = "UK", users = (int)(totalUsers * 0.15), revenue = 18000m },
                    new { country = "Canada", users = (int)(totalUsers * 0.12), revenue = 12000m },
                    new { country = "Australia", users = (int)(totalUsers * 0.08), revenue = 8000m }
                };

                return Ok(countries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Analytics/PerformanceMetrics/{userId}
        [HttpGet("PerformanceMetrics/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPerformanceMetrics(int userId)
        {
            try
            {
                // Get user's games
                var userGames = await _context.Games.Where(g => g.UserId == userId).ToListAsync();

                // Get total revenue
                var totalRevenue = await _context.TblPayments
                    .Where(p => _context.Games.Any(g => g.GameId == p.GameId && g.UserId == userId) &&
                               p.PaymentStatus == PaymentStatus.Completed)
                    .SumAsync(p => p.Amount);

                // Get average rating from reviews
                var avgRating = await _context.TblReviews
                    .Where(r => _context.Games.Any(g => g.GameId == r.GameId && g.UserId == userId))
                    .AverageAsync(r => (double?)r.Rating) ?? 3.5;

                // Calculate user retention (simulated)
                var retention = 85; // Placeholder value

                // Calculate revenue growth (simulated)
                var revenueGrowth = 65; // Placeholder value

                // Calculate metrics for radar chart
                var metrics = new[]
                {
                    new { metric = "User Retention", value = retention, fullMark = 100 },
                    new { metric = "Revenue Growth", value = revenueGrowth, fullMark = 100 },
                    new { metric = "Game Rating", value = (int)(avgRating / 5 * 100), fullMark = 100 },
                    new { metric = "Game Count", value = Math.Min(100, userGames.Count * 20), fullMark = 100 },
                    new { metric = "Market Share", value = userGames.Count > 2 ? 65 : 35, fullMark = 100 },
                    new { metric = "Active Games", value = userGames.Count(g => g.Status) * 25, fullMark = 100 }
                };

                return Ok(metrics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Analytics/BookingTrends/{userId}
        [HttpGet("BookingTrends/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetBookingTrends(int userId)
        {
            try
            {
                var trends = await (
                    from booking in _context.TblBookings
                    join game in _context.Games on booking.GameId equals game.GameId
                    where game.UserId == userId
                    group booking by booking.BookingDate.Date into dateGroup
                    orderby dateGroup.Key
                    select new
                    {
                        date = dateGroup.Key.ToString("yyyy-MM-dd"),
                        bookings = dateGroup.Count(),
                        revenue = dateGroup.Sum(b => b.Price)
                    }
                ).ToListAsync();

                return Ok(trends);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Analytics/ReviewAnalytics/{userId}
        [HttpGet("ReviewAnalytics/{userId}")]
        public async Task<ActionResult<object>> GetReviewAnalytics(int userId)
        {
            try
            {
                var reviews = await (
                    from review in _context.TblReviews
                    join game in _context.Games on review.GameId equals game.GameId
                    where game.UserId == userId
                    group review by review.Rating into ratingGroup
                    select new
                    {
                        rating = ratingGroup.Key,
                        count = ratingGroup.Count()
                    }
                ).ToListAsync();

                var avgRating = await _context.TblReviews
                    .Where(r => _context.Games.Any(g => g.GameId == r.GameId && g.UserId == userId))
                    .AverageAsync(r => (double?)r.Rating) ?? 0;

                return Ok(new { reviews, averageRating = Math.Round(avgRating, 2) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Analytics/UserActivity
        [HttpGet("UserActivity")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserActivity()
        {
            try
            {
                // Since we don't have a CreatedDate in the Tblusers model,
                // we'll use booking dates to simulate user activity
                var userActivity = await _context.TblBookings
                    .GroupBy(b => b.BookingDate.Date)
                    .Select(g => new
                    {
                        date = g.Key.ToString("yyyy-MM-dd"),
                        activeUsers = g.Select(b => b.UserId).Distinct().Count(),
                        sessions = g.Count()
                    })
                    .OrderBy(x => x.date)
                    .ToListAsync();
                    
                return Ok(userActivity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }







        // CAPTCHA Generation Endpoint
        [HttpGet("generate-captcha")]
        public IActionResult GenerateCaptcha()
        {
            try
            {
                // Generate random CAPTCHA code
                var random = new Random();
                var captchaCode = random.Next(100000, 999999).ToString();

                // Store CAPTCHA in session with timestamp
                HttpContext.Session.SetString("CaptchaCode", captchaCode);
                HttpContext.Session.SetString("CaptchaTime", DateTime.UtcNow.ToString());

                // Create image
                var width = 200;
                var height = 60;
                using var bitmap = new Bitmap(width, height);
                using var graphics = Graphics.FromImage(bitmap);

                // Background
                graphics.Clear(Color.FromArgb(247, 174, 71));

                // Add some noise
                var noiseRandom = new Random();
                for (int i = 0; i < 100; i++)
                {
                    var noiseX = noiseRandom.Next(width);
                    var noiseY = noiseRandom.Next(height);
                    var noiseColor = Color.FromArgb(noiseRandom.Next(200, 255), noiseRandom.Next(200, 255), noiseRandom.Next(200, 255));
                    bitmap.SetPixel(noiseX, noiseY, noiseColor);
                }

                // Add some lines
                using var pen = new Pen(Color.FromArgb(150, 150, 150), 2);
                for (int i = 0; i < 5; i++)
                {
                    graphics.DrawLine(pen,
                        noiseRandom.Next(width), noiseRandom.Next(height),
                        noiseRandom.Next(width), noiseRandom.Next(height));
                }

                // Add text
                using var font = new Font("Arial", 24, FontStyle.Bold);
                using var brush = new SolidBrush(Color.Black);

                var textSize = graphics.MeasureString(captchaCode, font);
                var textX = (width - textSize.Width) / 2;
                var textY = (height - textSize.Height) / 2;

                // Add each character with slight random positioning
                for (int i = 0; i < captchaCode.Length; i++)
                {
                    var charX = textX + (i * (textSize.Width / captchaCode.Length)) + noiseRandom.Next(-3, 3);
                    var charY = textY + noiseRandom.Next(-5, 5);
                    graphics.DrawString(captchaCode[i].ToString(), font, brush, charX, charY);
                }

                // Convert to byte array
                using var stream = new MemoryStream();
                bitmap.Save(stream, ImageFormat.Jpeg);
                var imageBytes = stream.ToArray();

                return File(imageBytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error generating CAPTCHA: {ex.Message}" });
            }
        }

        // CAPTCHA Verification Endpoint (Keep only this one)
        [HttpPost("verify-captcha")]
        public IActionResult VerifyCaptcha([FromBody] VerifyCaptchaRequest request)
        {
            try
            {
                var sessionCaptcha = HttpContext.Session.GetString("CaptchaCode");
                var captchaTimeStr = HttpContext.Session.GetString("CaptchaTime");

                if (string.IsNullOrEmpty(sessionCaptcha) || string.IsNullOrEmpty(captchaTimeStr))
                {
                    return BadRequest(new { success = false, message = "No CAPTCHA found. Please refresh the CAPTCHA." });
                }

                // Check if CAPTCHA has expired (5 minutes)
                if (DateTime.TryParse(captchaTimeStr, out var captchaTime))
                {
                    if ((DateTime.UtcNow - captchaTime).TotalMinutes > 5)
                    {
                        HttpContext.Session.Remove("CaptchaCode");
                        HttpContext.Session.Remove("CaptchaTime");
                        return BadRequest(new { success = false, message = "CAPTCHA has expired. Please refresh the CAPTCHA." });
                    }
                }

                // Verify CAPTCHA (case-insensitive)
                if (string.Equals(request.Captcha?.Trim(), sessionCaptcha, StringComparison.OrdinalIgnoreCase))
                {
                    // Mark CAPTCHA as verified in session
                    HttpContext.Session.SetString("CaptchaVerified", "true");
                    HttpContext.Session.SetString("CaptchaVerifiedTime", DateTime.UtcNow.ToString());

                    // Clear the CAPTCHA code after successful verification
                    HttpContext.Session.Remove("CaptchaCode");
                    HttpContext.Session.Remove("CaptchaTime");

                    return Ok(new { success = true, message = "CAPTCHA verified successfully" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Invalid CAPTCHA. Please try again." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error verifying CAPTCHA: {ex.Message}" });
            }
        }

        // Check Username Availability
        [HttpGet("check-username/{username}")]
        public async Task<IActionResult> CheckUsernameAvailability(string username)
        {
            try
            {
                var exists = await _context.Tblusers.AnyAsync(u => u.UserName == username);
                return Ok(new { isAvailable = !exists });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Check Email Availability
        [HttpGet("check-email/{email}")]
        public async Task<IActionResult> CheckEmailAvailability(string email)
        {
            try
            {
                var exists = await _context.Tblusers.AnyAsync(u => u.Email == email);
                return Ok(new { isAvailable = !exists });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Nested class for CAPTCHA verification
        public class VerifyCaptchaRequest
        {
            public string Captcha { get; set; }
        }





    }
}
