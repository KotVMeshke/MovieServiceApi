using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApi.DataBase.Entities;
using MovieServiceApi.Users.DTO;
using MovieServiceApi.Users.Services;
using MovieServiceApi.Utils.Policies;
using System.Security.Claims;

namespace MovieServiceApi.Users.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUser(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/login", UserAuthentication)
                .WithOpenApi();
            builder.MapPost("/registration", RegisterUser)
                .WithOpenApi();
            builder.MapPatch("/{userId:int}/ban", BanUser)
               .WithOpenApi();
            builder.MapPatch("/{userId:int}/unban", UnBanUser)
                .WithOpenApi();
            builder.MapGet("/",FindUsers)
                .WithOpenApi();
            builder.MapGet("/{userId:int}", FindUser)
                .WithOpenApi();
        }

        [Authorize(Policy = $"{PolicyType.AdministratorPolicy}")]
        private static async Task<IResult> BanUser(HttpContext context, [FromServices] UserService service, int userId, [FromQuery] int adminId)
        {
            if (context.User.FindFirstValue(ClaimTypes.NameIdentifier) != adminId.ToString()) return Results.UnprocessableEntity("Incorrect admin id");
            var banResult = await service.BanUser(userId, adminId);
            return banResult ? Results.Ok() : Results.UnprocessableEntity();
        }

        [Authorize(Policy = $"{PolicyType.AdministratorPolicy}")]
        private static async Task<IResult> UnBanUser(HttpContext context, [FromServices] UserService service, int userId, [FromQuery] int adminId)
        {
            if (context.User.FindFirstValue(ClaimTypes.NameIdentifier) != adminId.ToString()) return Results.UnprocessableEntity("Incorrect admin id");
            var banResult = await service.UnBanUser(userId, adminId);
            return banResult ? Results.Ok() : Results.UnprocessableEntity();
        }



        private async static Task<IResult> UserAuthentication([FromServices] UserService service, [FromBody] AuthenticationDTO dto)
        {
            var token = await service.AuthenticationUser(dto);
            return token is not null ? Results.Ok(new { acces_token = token }) : Results.Unauthorized();
        }

        private async static Task<IResult> RegisterUser([FromServices] UserService service, [FromBody] RegistrationDTO dto)
        {
            var token = await service.RegisterUser(dto);
            return token is not null ? Results.Ok(new { dto.Username, dto.Email, acces_token = token }) : Results.BadRequest();
        }

        [Authorize(Policy = $"{PolicyType.AdministratorPolicy}")]
        private async static Task<IResult> FindUser([FromServices] UserService service, int userId)
        {
            var user = await service.FindUser(userId);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        }

        [Authorize(Policy = $"{PolicyType.AdministratorPolicy}")]
        private async static Task<IResult> FindUsers([FromServices] UserService service, [AsParameters] UserFilterDTO dto)
        {
            var users = await service.FindUsers(dto);
            return users is not null ? Results.Ok(users) : Results.NotFound();
        }





    }
}
