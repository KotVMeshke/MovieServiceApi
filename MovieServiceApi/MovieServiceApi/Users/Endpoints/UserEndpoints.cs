using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApi.Users.DTO;
using MovieServiceApi.Users.Services;
using MovieServiceApi.Utils.Policies;

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
        }

        private async static Task<IResult> UserAuthentication([FromServices] UserService service, [FromBody] AuthenticationDTO dto)
        {
            var token = await service.AuthenticationUser(dto);
            return token is not null ? Results.Ok(new { acces_token = token }) : Results.Unauthorized();
        }

        private async static Task<IResult> RegisterUser([FromServices] UserService service, [FromBody] RegistrationDTO dto)
        {
            var token = await service.RegisterUser(dto);
            return token is not null ? Results.Ok(new {dto.Username, dto.Email, acces_token = token}) : Results.BadRequest();
        }
    }
}
