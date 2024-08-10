using System;
using System.Collections.Generic;

namespace MovieServiceApi.DataBase.Entities;

public partial class History
{
    public DateTime? HData { get; set; }

    public int? HUser { get; set; }

    public int? HFilm { get; set; }

    public virtual Film? HFilmNavigation { get; set; }

    public virtual User? HUserNavigation { get; set; }
}
