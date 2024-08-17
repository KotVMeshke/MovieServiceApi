using Microsoft.EntityFrameworkCore;
using MovieServiceApi.Persons.DTO;
using MovieServiceApi.DataBase.Context;

namespace MovieServiceApi.Persons.Services
{
    public class PersonService(ILogger<PersonService> logger, MovieServiceContext db)
    {
        public async Task<FullPersonMemeberDTO?> GetPersonMember(int crewId)
        {
			try
			{
                var crewMember = await db.People
                    .Include(p => p.Photos)
                    .Where(p => p.PerId == crewId)
                    .Select(p => new FullPersonMemeberDTO()
                    {
                        Age = p.PerAge,
                        Date = p.PerBirthDate,
                        Name = p.PerName,
                        Patronymic = p.PerPatronymic,
                        PhotoPath = (p.Photos.FirstOrDefault() ?? new DataBase.Entities.Photo()).PhPath
                    })
                    .FirstOrDefaultAsync();

                return crewMember;
			}
			catch (Exception ex)
			{
                logger.LogError("Exceptions occured during person info {id} getting: {ex}", crewId, ex);
                return null;
			}
        }
    }
}
