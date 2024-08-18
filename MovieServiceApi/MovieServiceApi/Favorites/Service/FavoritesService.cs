using Microsoft.EntityFrameworkCore;
using MovieServiceApi.DataBase.Context;
using MovieServiceApi.Movies.DTO;
using System.Diagnostics;

namespace MovieServiceApi.Favorites.Service
{
    public class FavoritesService(ILogger<FavoritesService> logger, MovieServiceContext db)
    {
        public async Task<List<MovieResponceDTO>?> GetFavorites(int userId)
        {
            try
            {
                var filmIds = await db.Users
                    .Where(u => u.UsrId == userId)
                    .Include(u => u.LibFilms)
                    .SelectMany(f => f.LibFilms, (u, f) => f.FlmId)
                    .ToListAsync();

                var films = await db.FilmInfos
                    .Where(f => filmIds.Contains(f.Id))
                    .Select(f => new MovieResponceDTO()
                    {
                        Id = f.Id,
                        Age = f.Age,
                        CountryName = f.CountryName,
                        Description = f.Description,
                        FilmPath = f.FilmPath,
                        Name = f.Name,
                        ReleaseDate = f.ReleaseDate,
                        TrailerPath = f.TrailerPath,
                        PosterPath = new string[] { f.PosterFilepath }
                    })
                    .ToListAsync();

                return films;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured during getting favorite films for user {id}: {ex}", userId, ex);
                return null;
            }
        }

        public async Task<bool> AddFilmIn(int userId, int filmId)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();
                //var user = await db.Users
                //    .FirstOrDefaultAsync(u => u.UsrId == userId);
                //var film = await db.Films
                //    .FirstOrDefaultAsync(f => f.FlmId == filmId);
                //if (user is null || film is null) return false;
                //user.LibFilms.Add(film);
                //int numberOfInserted = db.SaveChanges();
                int numberOfInserted = await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO [library] (lib_user, lib_film) VALUES ({userId},{filmId})");
                watch.Stop();
                Console.WriteLine(watch.Elapsed);
                if (numberOfInserted == 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured during addition favorite film {filmId} into  user's favorites {userId}: {ex}", filmId,userId, ex);
                return false;
            }
        }

        public async Task<bool> DeleteFilmFrom(int userId, int filmId)
        {
            try
            {
                var numberOfDeleted = await db.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM [library] WHERE [lib_user] = {userId} AND [lib_film] = {filmId}");
                if (numberOfDeleted == 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured during deletion favorite film {filmId} into  user's favorites {userId}: {ex}", filmId,userId, ex);
                return false;
            }
        }
    }
}
