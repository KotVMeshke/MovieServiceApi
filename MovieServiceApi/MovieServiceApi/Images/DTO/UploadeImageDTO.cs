namespace MovieServiceApi.Images.DTO
{
    public class UploadeImageDTO
    {
        public string PathInImages { get; set; } = "";
        public IFormFile File { get; set; } = null!;
    }
}
