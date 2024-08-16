using Microsoft.EntityFrameworkCore;
using MovieServiceApi.DataBase.Context;
using MovieServiceApi.DataBase.Entities;
using MovieServiceApi.Movies.DTO;
using MovieServiceApi.Users.Services;
using MovieServiceApi.Crew.DTO;
using MovieServiceApi.Genres.DTO;

namespace MovieServiceApi.Movies.Services
{
    public class MovieService(ILogger<MovieService> logger, MovieServiceContext db, IConfiguration config)
    {
        public async Task<List<MovieResponceDTO>?> GetFilms(MovieFilterDTO dto)
        {
			try
			{
				IQueryable<FilmInfo> query = CreateQuery(dto);

				var films = await query.Select(f => new MovieResponceDTO()
				{
					Id = f.Id,
					Age = f.Age,
					CountryName = f.CountryName,
					Description = f.Description,
					FilmPath = f.FilmPath,
					Name = f.Name,
					ReleaseDate = f.ReleaseDate,
					TrailerPath = f.TrailerPath,
					PosterPath =  new string[] { f.PosterFilepath }
					
				}).ToListAsync();
				return films;
			}
			catch (Exception ex)
			{
				logger.LogError("Exceptions occured during film finding {exception}", ex);
				return null;
			}
        }

        public async Task<MovieResponceDTO?> GetFilm(int filmId)
        {
            try
            {
				var film = await db.FilmInfos
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
					.FirstOrDefaultAsync(f => f.Id == filmId);
				if (film is null) return null;
                logger.LogInformation("Film {id} was getted succesfuly", film.Id);

                return film;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during film finding {exception}", ex);
                return null;
            }
        }

        public async Task<bool?> CreateFilm()
		{
			throw new NotImplementedException();
		}

		public async Task<bool?> DeleteFilm(int filmId)
		{
			try
			{
				int numberOfDeleted = await db.Films.Where(u => u.FlmId == filmId).ExecuteDeleteAsync();
				return numberOfDeleted > 0 ? true : false;
			}
			catch (Exception ex)
			{
                logger.LogError("Exceptions occured during films deleting {exception}", ex);
                return null;
			}
		}

		public async Task<List<ShortCrewMemeberDTO>?> GetFilmCrew(int filmId)
		{
			try
			{
				var crew = await db.FilmCrews
					.Where(f => f.FcrFilm == filmId)
					.Include(f => f.FcrPersonNavigation)
					.Include(f => f.FcrRoleNavigation)
					.Select(f => new ShortCrewMemeberDTO()
					{
						Id = f.FcrPerson,
						Name = f.FcrPersonNavigation.PerName,
						Role = f.FcrRoleNavigation.CrName,
						Surname = f.FcrPersonNavigation.PerSurname
					})
					.ToListAsync();

				return crew;
			}
			catch (Exception ex)
			{
                logger.LogError("Exceptions occured during film's crew getting {exception}", ex);
                return null;
            }
		}

		public async Task<List<GenresResponceDTO>?> GetFilmGenres(int filmId)
		{
			try
			{
				var genres = await db.Films
					.Where(f => f.FlmId == filmId)
					.Include(f => f.FgGenres)
					.SelectMany(f => f.FgGenres)
					.Select(g => new GenresResponceDTO()
					{
						Name = g.GnrName
					})
					.ToListAsync();

				return genres;
			}
			catch (Exception ex)
			{
                logger.LogError("Exceptions occured during film's genres getting {exception}", ex);
                return null;
            }
		}
		private IQueryable<FilmInfo> CreateQuery(MovieFilterDTO dto)
		{
			IQueryable<FilmInfo> query = db.FilmInfos;
			
			if (dto.Name is not null) query = query.Where(f => EF.Functions.Like(f.Name, $"%{dto.Name}%"));
			//if (dto.ReleaseDate is not null) query = query.Where(f => f.FlmReleaseDate == dto.ReleaseDate.Value);//Change to Realese time between start and end in future
			if (dto.Age is not null) query = query.Where(f => f.Age == dto.Age);
			if (dto.CountryName is not null) query = query.Where(f => f.CountryName == dto.CountryName);


			return query;
		}
    }
}
