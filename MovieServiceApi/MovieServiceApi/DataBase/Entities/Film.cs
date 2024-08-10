using System;
using System.Collections.Generic;

namespace MovieServiceApi.DataBase.Entities;

public partial class Film
{
    public int FlmId { get; set; }

    public string FlmName { get; set; } = null!;

    public DateOnly FlmReleaseDate { get; set; }

    public string FlmDescription { get; set; } = null!;

    public int FlmAgeRestriction { get; set; }

    public int? FlmMedia { get; set; }

    public int FlmCountry { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<FilmCrew> FilmCrews { get; set; } = new List<FilmCrew>();

    public virtual AgeRestriction FlmAgeRestrictionNavigation { get; set; } = null!;

    public virtual Country FlmCountryNavigation { get; set; } = null!;

    public virtual Medium? FlmMediaNavigation { get; set; }

    public virtual ICollection<Voiceover> Voiceovers { get; set; } = new List<Voiceover>();

    public virtual ICollection<Genre> FgGenres { get; set; } = new List<Genre>();

    public virtual ICollection<User> LibUsers { get; set; } = new List<User>();
}
