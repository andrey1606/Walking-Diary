using System;
using System.Collections.Generic;

namespace WalkingDiary;

public partial class Walk
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateOnly Date { get; set; }

    public float AirTemperature { get; set; }

    public TimeSpan Duration { get; set; }

    public float Distance { get; set; }

    public string WalkType { get; set; } = null!;

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<WalkPhoto> WalkPhotos { get; } = new List<WalkPhoto>();
}
