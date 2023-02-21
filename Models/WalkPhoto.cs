using System;
using System.Collections.Generic;

namespace WalkingDiary;

public partial class WalkPhoto
{
    public int Id { get; set; }

    public int WalkId { get; set; }

    public string Url { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Walk Walk { get; set; } = null!;
}
