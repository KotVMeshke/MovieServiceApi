using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApi.DataBase.Entities;
using MovieServiceApi.Feedbacks.DTO;
using MovieServiceApi.Feedbacks.Service;
using MovieServiceApi.Utils.Policies;

namespace MovieServiceApi.Feedbacks.Endpoints
{
    public static class FeedbackEndpoints
    {
        public static void MapFeedback(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/film/{filmId:int}", GetFeedbackForFilm)
                .WithOpenApi();
            builder.MapGet("/user/{userId:int}", GetFeedbackForUser)
                .WithOpenApi();
            builder.MapPost("/", CreateFeedback)
                .WithOpenApi();
            builder.MapDelete("/", DeleteFeedback)
                .WithOpenApi();
            //builder.MapPut("/", UpdateFeedback)
            //    .WithOpenApi();

        }

        [Authorize(Policy = $"{PolicyType.RegularUserPolicy}")]
        public static async Task<IResult> CreateFeedback([FromServices] FeedbackService service, [FromBody] FeedbackParamsDTO fParams)
        {
            var isCreated = await service.CreateFeedBack(fParams);
            return isCreated ? Results.Created() : Results.NoContent();
        }

        [Authorize(Policy = $"{PolicyType.RegularUserPolicy}")]
        public static async Task<IResult> GetFeedbackForFilm([FromServices] FeedbackService service,int filmId )
        {
            var feedback = await service.GetFeedbackForFilm(filmId);
            return feedback is not null ? Results.Ok(feedback) : Results.NotFound();

        }

        [Authorize(Policy = $"{PolicyType.RegularUserPolicy}")]
        public static async Task<IResult> GetFeedbackForUser([FromServices] FeedbackService service, int userId)
        {
            var feedback = await service.GetFeedbackForUser(userId);
            return feedback is not null ? Results.Ok(feedback) : Results.NotFound();
        }

        [Authorize(Policy = $"{PolicyType.RegularUserPolicy}")]
        public static async Task<IResult> DeleteFeedback([FromServices] FeedbackService service, int filmId, int userId)
        {
            var isDeleted = await service.DeleteFeedback(filmId, userId);
            return isDeleted ? Results.Ok() : Results.BadRequest();
        }

        //Forbidden
        public static async Task<IResult> UpdateFeedback([FromServices] FeedbackService service,[FromBody] FeedbackUpdateDTO updateDTO)
        {
            var isUpdated = await service.UpdateFeedback(updateDTO);
            return isUpdated ? Results.Ok() : Results.BadRequest();
        }
    }
}
