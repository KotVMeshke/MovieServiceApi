using Microsoft.EntityFrameworkCore;
using MovieServiceApi.DataBase.Context;
using System.Reflection;

namespace MovieServiceApi.Administrator.Services
{
    public class AdminService(ILogger<AdminService> logger, MovieServiceContext db)
    {
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
                logger.LogError("Exception was occured during the user banning process: {ex}",ex);
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
    }
}
