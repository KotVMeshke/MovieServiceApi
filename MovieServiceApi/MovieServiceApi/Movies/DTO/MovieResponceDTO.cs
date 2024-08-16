namespace MovieServiceApi.Movies.DTO
{
    public class MovieResponceDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public string? Age { get; set; }
        public string? TrailerPath { get; set; }
        public string? FilmPath { get; set; }
        public string? CountryName { get; set; }
        public string[]? PosterPath { get; set; }
    }
}
