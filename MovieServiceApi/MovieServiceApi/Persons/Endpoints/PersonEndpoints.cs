using Microsoft.AspNetCore.Mvc;
using MovieServiceApi.Persons.Services;

namespace MovieServiceApi.Persons.Endpoints
{
    public static class PersonEndpoints
    {
        public static void MapCrew(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/{personId:int}", GetPerson)
                .WithOpenApi();
        }

        private static async Task<IResult> GetPerson([FromServices] PersonService service, int personId)
        {
            var personMemeber = await service.GetPersonMember(personId);

            return personMemeber is not null ? Results.Ok(personMemeber) : Results.NotFound();
        }
    }
}
