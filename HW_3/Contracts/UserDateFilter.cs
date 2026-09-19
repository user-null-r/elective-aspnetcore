using System.ComponentModel.DataAnnotations;

namespace HW_3.Contracts;

public sealed class UserDateFilter : IValidatableObject
{
    public DateTimeOffset? CreatedFrom { get; init; }
    public DateTimeOffset? CreatedTo { get; init; }
    public DateTimeOffset? UpdatedFrom { get; init; }
    public DateTimeOffset? UpdatedTo { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CreatedFrom > CreatedTo)
            yield return new ValidationResult(
                "CreatedFrom не может быть позже CreatedTo.",
                [nameof(CreatedFrom), nameof(CreatedTo)]);
        if (UpdatedFrom > UpdatedTo)
            yield return new ValidationResult(
                "UpdatedFrom не может быть позже UpdatedTo.",
                [nameof(UpdatedFrom), nameof(UpdatedTo)]);
    }
}

