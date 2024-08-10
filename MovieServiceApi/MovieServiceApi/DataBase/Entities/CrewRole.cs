using System;
using System.Collections.Generic;

namespace MovieServiceApi.DataBase.Entities;

public partial class CrewRole
{
    public int CrId { get; set; }

    public string CrName { get; set; } = null!;

    public virtual ICollection<FilmCrew> FilmCrews { get; set; } = new List<FilmCrew>();
}
