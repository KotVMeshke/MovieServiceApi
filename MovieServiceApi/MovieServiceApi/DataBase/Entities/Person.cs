using System;
using System.Collections.Generic;

namespace MovieServiceApi.DataBase.Entities;

public partial class Person
{
    public int PerId { get; set; }

    public string PerName { get; set; } = null!;

    public string PerSurname { get; set; } = null!;

    public string? PerPatronymic { get; set; }

    public int PerAge { get; set; }

    public DateOnly PerBirthDate { get; set; }

    public virtual ICollection<FilmCrew> FilmCrews { get; set; } = new List<FilmCrew>();

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
