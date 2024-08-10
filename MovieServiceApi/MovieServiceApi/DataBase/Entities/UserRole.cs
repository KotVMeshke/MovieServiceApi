using System;
using System.Collections.Generic;

namespace MovieServiceApi.DataBase.Entities;

public partial class UserRole
{
    public int UrId { get; set; }

    public string UrName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
