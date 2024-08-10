using System;
using System.Collections.Generic;

namespace MovieServiceApi.DataBase.Entities;

public partial class Poster
{
    public int PstId { get; set; }

    public string PstFilepath { get; set; } = null!;

    public virtual ICollection<Medium> MpMedia { get; set; } = new List<Medium>();
}
