using System;
using System.Collections.Generic;

namespace MovieServiceApi.DataBase.Entities;

public partial class FilmInfo
{
    public string Name { get; set; } = null!;

    public DateOnly ReleaseDate { get; set; }

    public string Description { get; set; } = null!;

    public string Age { get; set; } = null!;

    public string TrailerPath { get; set; } = null!;

    public string? FilmPath { get; set; }

    public string CountryName { get; set; } = null!;

    public int Id { get; set; }

    public string PosterFilepath { get; set; } = null!;
}
