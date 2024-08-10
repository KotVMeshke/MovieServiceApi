using System;
using System.Collections.Generic;

namespace MovieServiceApi.DataBase.Entities;

public partial class AgeRestriction
{
    public int AgeId { get; set; }

    public string AgeValue { get; set; } = null!;

    public virtual ICollection<Film> Films { get; set; } = new List<Film>();
}
