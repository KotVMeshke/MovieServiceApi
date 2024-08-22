using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieServiceApi.Videos.Service;
using MovieServiceApi.Videos.DTO;

namespace MovieServiceApi.Videos.Endpoints
{
    public static class VideoEndpoints
    {
        public static void MapVideo(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/", GetVideo)
                .WithOpenApi();
            builder.MapPost("/", AddIntoDataBase)
                .WithOpenApi();
        }

        private static async Task<IResult> GetVideo(HttpContext context,[FromServices] VideoService service, [FromQuery] string path)
        {
            var fileInfo = await service.GetVideoFile(path, context);

            if (fileInfo.Value.file is null) return Results.NoContent();
            if (!fileInfo.Value.isPartitial) return Results.File(fileInfo.Value.file, "video/mp4");
            return Results.File(fileInfo.Value.file, "application/octet-stream", enableRangeProcessing: true);
        }

        private static async Task<IResult> AddIntoDataBase([FromServices] VideoService service, [FromBody] VideoInsertDTO dto)
        {
            var isAdded = await service.AddIntoDataBase(dto);

            return isAdded ? Results.Created() : Results.BadRequest();
        }
    }
}
