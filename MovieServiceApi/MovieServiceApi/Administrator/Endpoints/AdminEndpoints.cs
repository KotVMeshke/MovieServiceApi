using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApi.Administrator.Services;
using MovieServiceApi.Utils.Policies;
using System.Security.Claims;

namespace MovieServiceApi.Administrator.Endpoints
{
    public static class AdminEndpoints
    {
        public static void MapAdministrator(this IEndpointRouteBuilder builder)
        {
            builder.MapPatch("/ban/{userId:int}",BanUser)
                .WithOpenApi();
            builder.MapPatch("/unban/{userId:int}",UnBanUser)
                .WithOpenApi();
        }

        [Authorize(Policy = $"{PolicyType.AdministratorPolicy}")]
        private static async Task<IResult> BanUser(HttpContext context, [FromServices] AdminService service,int userId,[FromQuery] int adminId)
        {
            if (context.User.FindFirstValue(ClaimTypes.NameIdentifier) != adminId.ToString()) return Results.UnprocessableEntity("Incorrect admin id");
            var banResult = await service.BanUser(userId, adminId);
            return banResult ? Results.Ok() : Results.UnprocessableEntity();
        }

        [Authorize(Policy = $"{PolicyType.AdministratorPolicy}")]
        private static async Task<IResult> UnBanUser(HttpContext context, [FromServices] AdminService service, int userId, [FromQuery] int adminId)
        {
            if (context.User.FindFirstValue(ClaimTypes.NameIdentifier) != adminId.ToString()) return Results.UnprocessableEntity("Incorrect admin id");
            var banResult = await service.UnBanUser(userId, adminId);
            return banResult ? Results.Ok() : Results.UnprocessableEntity();
        }
    }
}
