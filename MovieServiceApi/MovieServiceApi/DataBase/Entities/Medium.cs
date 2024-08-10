using System;
using System.Collections.Generic;

namespace MovieServiceApi.DataBase.Entities;

public partial class Medium
{
    public int MedId { get; set; }

    public string MedTrailerPath { get; set; } = null!;

    public string? MedFilmPath { get; set; }

    public virtual ICollection<Film> Films { get; set; } = new List<Film>();

    public virtual ICollection<Poster> MpPosters { get; set; } = new List<Poster>();
}
