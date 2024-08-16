namespace MovieServiceApi.Movies.DTO
{
    public class MovieFilterDTO
    {
        public string? Name { get; set; }
        //Find out how to add date searching
        //public DateOnly? ReleaseDate { get; set; }
        public string? Age { get; set; }
        public string? CountryName { get; set; }
    }
}
