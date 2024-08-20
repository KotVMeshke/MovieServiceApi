using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApi.Images.DTO;
using MovieServiceApi.Images.Service;
using MovieServiceApi.Utils.Policies;

namespace MovieServiceApi.Images.Endpoints
{
    public static class ImageEndpoints
    {
        public static void MapImage(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/", GetImage)
                .WithOpenApi();
            builder.MapPost("/", UploadImage)
                .WithOpenApi()
                .DisableAntiforgery();// Change it later
        }

        public static async Task<IResult> GetImage([FromServices] ImageService service, string path)
        {
            var image = await service.GetImage(path);
            return image is not null ? Results.File(image, "image/jpeg") : Results.NotFound();
        }

        [Authorize(Policy = $"{PolicyType.AdministratorPolicy}")]
        public static async Task<IResult> UploadImage([FromServices] ImageService service, [FromForm(Name = "file")] IFormFile file, [FromQuery] string pathInImages)
        {
            UploadeImageDTO dto = new UploadeImageDTO()
            {
                File = file,
                PathInImages = pathInImages
            };
            var isUploaded = await service.UploadImage(dto);
            return isUploaded ? Results.Ok() : Results.BadRequest();
        }
    }
}
