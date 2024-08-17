namespace MovieServiceApi.Movies.DTO
{
    public class MovieFilterDTO
    {
        public string? Name { get; set; }
        //Find out how to add date searching
        public int? StartYear { get; set; }
        public int? StartMonth { get; set; }
        public int? StartDay { get; set; }
        public int? EndYear { get; set; }
        public int? EndMonth { get; set; }
        public int? EndDay { get; set; }
        public string? Age { get; set; }
        public string? CountryName { get; set; }
    }
}
