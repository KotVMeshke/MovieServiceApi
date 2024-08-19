namespace MovieServiceApi.Feedbacks.DTO
{
    public class FeedbackParamsDTO
    {
        public string? Text { get; set; } = "";
        public int Rating { get; set; }
        public int FilmId { get; set; }
        public int UserId { get; set; }
    }
}
