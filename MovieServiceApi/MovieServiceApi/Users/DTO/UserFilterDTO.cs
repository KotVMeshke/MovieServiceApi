using MovieServiceApi.DataBase.Entities;

namespace MovieServiceApi.Users.DTO
{
    public class UserFilterDTO
    {
        public int? Id { get; set; } 
        public string? UserName { get; set; }
        public bool? IsBanned { get; set; }
        public string? Role { get; set; }
    }
}
