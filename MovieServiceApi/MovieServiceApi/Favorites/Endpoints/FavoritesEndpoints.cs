using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApi.Favorites.Service;
using MovieServiceApi.Utils.Policies;
using System.Security.Claims;

namespace MovieServiceApi.Favorites.Endpoints
{
    public static class FavoritesEndpoints
    {
        public static void MapFavorites(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/{userId:int}", GetFavorites)
                .WithOpenApi();
            builder.MapPost("/{userId:int}", AddFilmIn)
                .WithOpenApi();
            builder.MapDelete("/{userId:int}", DeleteFilmFrom)
                .WithOpenApi();
        }

        [Authorize(Policy = $"{PolicyType.RegularUserPolicy}")]
        private static async Task<IResult> GetFavorites(HttpContext context, [FromServices] FavoritesService service, int userId)
        {
            if (context.User.FindFirstValue(ClaimTypes.NameIdentifier) != userId.ToString()) return Results.UnprocessableEntity("Incorrect user id");
            var films = await service.GetFavorites(userId);
            return films is not null ? Results.Ok(films) : Results.NotFound();
        }

        [Authorize(Policy = $"{PolicyType.RegularUserPolicy}")]
        private static async Task<IResult> AddFilmIn(HttpContext context, [FromServices] FavoritesService service, int userId, [FromQuery] int filmId)
        {
            if (context.User.FindFirstValue(ClaimTypes.NameIdentifier) != userId.ToString()) return Results.UnprocessableEntity("Incorrect user id");
            var isAdded = await service.AddFilmIn(userId, filmId);
            return isAdded ? Results.Ok() : Results.BadRequest();
        }
        [Authorize(Policy =$"{PolicyType.RegularUserPolicy}")]
        private static async Task<IResult> DeleteFilmFrom(HttpContext context, [FromServices] FavoritesService service, int userId, [FromQuery] int filmId)
        {
            if (context.User.FindFirstValue(ClaimTypes.NameIdentifier) != userId.ToString()) return Results.UnprocessableEntity("Incorrect user id");
            var isDeleted = await service.DeleteFilmFrom(userId, filmId);
            return isDeleted ? Results.Ok() : Results.BadRequest();
        }
    }
}
