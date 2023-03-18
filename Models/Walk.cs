using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WalkingDiary;

public partial class Walk
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateOnly Date { get; set; }

    public float AirTemperature { get; set; }

    [DurationOrDistanceRequired]
    public TimeSpan? Duration { get; set; }

    [DurationOrDistanceRequired]
    public float? Distance { get; set; }

    public string WalkType { get; set; } = null!;

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public User? User { get; set; }

    public virtual ICollection<WalkPhoto> WalkPhotos { get; } = new List<WalkPhoto>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Duration == TimeSpan.Zero && Distance == 0)
        {
            yield return new ValidationResult("At least one of the fields 'Duration' or 'Distance' must be filled in.");
        }
    }
}

public class DurationOrDistanceRequiredAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var walk = (Walk)validationContext.ObjectInstance;

        if (walk.Duration == null && walk.Distance == null)
        {
            return new ValidationResult("Either Duration or Distance is required.");
        }

        return ValidationResult.Success;
    }
}

