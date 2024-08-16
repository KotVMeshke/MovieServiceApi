using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieServiceApi.Movies.DTO;
using MovieServiceApi.Movies.Services;
using MovieServiceApi.Utils.Policies;

namespace MovieServiceApi.Movies.Endpoints
{
    public static class MovieEndpoints
    {
        public static void MapMovies(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/{filmId:int}", GetFilm);
            builder.MapGet("/", GetFilms)
                .WithOpenApi();
            builder.MapGet("/{filmId:int}/crew", GetFilmCrew)
                .WithOpenApi();
            builder.MapGet("/{filmId:int}/genres", GetFilmsGenres)
                .WithOpenApi();
            builder.MapDelete("/", DeleteFilm)
                .WithOpenApi();
            builder.MapPost("/", CreateFilm)
                .WithOpenApi();
        }


        private static async Task<IResult> GetFilm([FromServices] MovieService service, int filmId)
        {
          
            var film = await service.GetFilm(filmId);

            return film is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(film);
        }
        private static async Task<IResult> GetFilms([FromServices] MovieService service, [AsParameters] MovieFilterDTO dto)
        {
            //if (dto.ReleaseDateStart is not null && dto.ReleaseDateEnd is not null && dto.ReleaseDateStart > dto.ReleaseDateEnd)
            //    return Results.StatusCode(StatusCodes.Status406NotAcceptable);

            var films = await service.GetFilms(dto);

            return films is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(films);
        }

        [Authorize(Policy = $"{PolicyType.AdministratorPolicy}")]
        private static async Task<IResult> DeleteFilm([FromServices] MovieService service, int filmId)
        {
            var isDeleted = await service.DeleteFilm(filmId);

            return isDeleted.Value ? Results.Ok() : Results.StatusCode(StatusCodes.Status400BadRequest);
        }

        //[Authorize(Policy = $"{PolicyType.AdministratorPolicy}")]
        private static async Task<IResult> CreateFilm([FromServices] MovieService service)
        {
            throw new NotImplementedException();
        }

        private static async Task<IResult> GetFilmCrew(int filmId, [FromServices] MovieService service)
        {
            var crew = await service.GetFilmCrew(filmId);
            return crew is not null ? Results.Ok(crew) : Results.NotFound();
        }
        
        private static async Task<IResult> GetFilmsGenres(int filmId ,[FromServices] MovieService service)
        {
            var genres = await service.GetFilmGenres(filmId);

            return genres is not null ? Results.Ok(genres) : Results.NotFound();
        }
    }
}
