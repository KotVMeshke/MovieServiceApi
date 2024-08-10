using System;
using System.Collections.Generic;

namespace MovieServiceApi.DataBase.Entities;

public partial class User
{
    public int UsrId { get; set; }

    public string UsrName { get; set; } = null!;

    public string UsrEmail { get; set; } = null!;

    public string UsrPassword { get; set; } = null!;

    public int UserRole { get; set; }

    public int? UserBannedBy { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<User> InverseUserBannedByNavigation { get; set; } = new List<User>();

    public virtual User? UserBannedByNavigation { get; set; }

    public virtual UserRole UserRoleNavigation { get; set; } = null!;

    public virtual ICollection<Film> LibFilms { get; set; } = new List<Film>();
}
