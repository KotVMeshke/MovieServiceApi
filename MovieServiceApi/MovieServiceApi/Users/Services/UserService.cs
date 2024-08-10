using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieServiceApi.DataBase.Context;
using MovieServiceApi.DataBase.Entities;
using MovieServiceApi.Users.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MovieServiceApi.Users.Services
{
    public class UserService(ILogger<UserService> logger, MovieServiceContext db, IConfiguration config)
    {
        public async Task<string?> AuthenticationUser(AuthenticationDTO dto)
        {
            try
            {
                var passwordHash = CreateHashCode(dto.Password!);
                var user = await db.Users.Include(u => u.UserRoleNavigation).FirstOrDefaultAsync(u => u.UsrPassword == passwordHash && u.UsrEmail == dto.Email);
                if (user is null) return null;
                logger.LogInformation("User {user} was authenticated", user.UsrName);
                return new JwtSecurityTokenHandler().WriteToken(CreateToken(user));
            }
            catch (Exception ex)
            {
                logger.LogError("Error has occured in user Authentication: {exception}",ex);
                return null;
            }
           
        }

        public async Task<string?> RegisterUser(RegistrationDTO dto)
        {
            try
            {
                var passwordHash = CreateHashCode(dto.Password);
                var user = new User { UsrEmail = dto.Email, UsrPassword = passwordHash, UserRole =  1, UsrName = dto.Username};
                db.Users.Add(user);
                int numberOfAdded = await db.SaveChangesAsync();
                if (numberOfAdded == 0) return null;
                await db.Entry(user).Reference(u => u.UserRoleNavigation).LoadAsync();
                logger.LogInformation("User {user} was registered", user.UsrName);

                return new JwtSecurityTokenHandler().WriteToken(CreateToken(user));
            }
            catch (Exception ex)
            {
                logger.LogError("Error has occured while user registration: {exception}", ex);
                return null;

            }
        }


        public async Task<bool> BanUser(int userId, int adminId)
        {
            try
            {
                int numberOfAffected = await db.Users.Where(u => u.UsrId == userId).ExecuteUpdateAsync(u => u.SetProperty(u => u.UserBannedBy, b => adminId));
                if (numberOfAffected == 0) return false;
                logger.LogInformation("User with id: {user} was banned by {admin}", userId, adminId);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception was occured during the user banning process: {ex}", ex);
                return false;
            }
        }

        public async Task<bool> UnBanUser(int userId, int adminId)
        {
            try
            {
                int numberOfAffected = await db.Users.Where(u => u.UsrId == userId).ExecuteUpdateAsync(u => u.SetProperty(u => u.UserBannedBy, b => null));
                if (numberOfAffected == 0) return false;
                logger.LogInformation("User with id: {user} was unbanned by {admin}", userId, adminId);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception was occured during the user unbanning process: {ex}", ex);
                return false;
            }
        }
        private JwtSecurityToken CreateToken(User user)
        {
            IEnumerable<Claim> claims = 
                [
                    new Claim(ClaimTypes.Name, user.UsrName),
                    new Claim(ClaimTypes.Role, user.UserRoleNavigation.UrName),
                    new Claim(ClaimTypes.NameIdentifier, user.UsrId.ToString())
                ];
            return new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
                issuer: config["JwtParameters:Issuer"],
                audience: config["JwtParameters:Audience"],
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtParameters:Key"]!)),
                    SecurityAlgorithms.HmacSha256)
            );
        }
        private string CreateHashCode(string input)
        {
            string hash = string.Empty;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                hash = builder.ToString();
            }
            return hash;
        }
    }
}
