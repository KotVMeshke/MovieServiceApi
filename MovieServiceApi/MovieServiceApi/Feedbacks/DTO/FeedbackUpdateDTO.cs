namespace MovieServiceApi.Feedbacks.DTO
{
    public class FeedbackUpdateDTO
    {
        public int UserId { get; set; }
        public int FilmId { get; set; }
        public int? NewRating { get; set; }
        public string? NewText {  get; set; } 
    }
}
