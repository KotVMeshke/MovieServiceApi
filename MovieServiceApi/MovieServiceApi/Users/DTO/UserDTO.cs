namespace MovieServiceApi.Users.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Role { get; set; }
        public bool BanStatus {  get; set; } 
    }
}
