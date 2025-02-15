﻿using BoatSea.Data;
using BoatSea.Interfaces;
using BoatSea.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BoatSea.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;
        public UserService(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _databaseContext.Users.ToListAsync();
        }

        public async Task DeleteUser(User user)
        {
            _databaseContext.Users.Remove(user);
             await _databaseContext.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _databaseContext.Users.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Auth:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        public string HashPassword(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                return String.Empty;
            }

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }
        public async Task RegisterUser(User user)
        {
            await _databaseContext.Users.AddAsync(user);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _databaseContext.Users.Where(e =>  e.Email == email).FirstOrDefaultAsync();
        }

        public string CreateRandomToken()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("medmedorl121@gmail.com", "jdoc jlyu ovmc inxf"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("medmedorl121@gmail.com"),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task UpdateUserAsync(User user)
        {
            _databaseContext.Update(user);
            await _databaseContext.SaveChangesAsync();
        }

        public string GenerateResetToken(User user)
        {
            var resetToken = Guid.NewGuid().ToString();
            user.ResetToken = resetToken;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(2);

            _databaseContext.Users.Update(user);
            _databaseContext.SaveChanges();

            return resetToken;
        }

        public async Task SendPasswordResetEmail(string email, string resetToken)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("medmedorl121@gmail.com"),
                Subject = "Reset Password",
                Body = $"<p>Hello,</p>" +
                        $"<p>You have requested to reset your password." +
                        $"<p>To continue, go to the password reset page in the app.</p>" +
                        $"<br><p>Best regards,<br>BoatSea Team</p>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("medmedorl121@gmail.com", "jdoc jlyu ovmc inxf");
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        public async Task<User> GetUserByResetToken(string token)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.ResetToken == token);

            if (user == null || user.ResetTokenExpiry < DateTime.UtcNow)
                return null;

            return user;
        }

        public async Task ResetPassword(User user, string newPassword)
        {
            user.Password = HashPassword(newPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<Drzava>> GetGrad(string drzava)
        {
            return await _databaseContext.Drzava.Where(d => d.Country == drzava).ToListAsync();
        }
    }
}
