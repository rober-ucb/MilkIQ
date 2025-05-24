using Cattle.Domain.Enums;

namespace Cattle.Domain.ValueObjects;

/// <summary>
/// Represents the life stage of an animal based on its date of birth.
/// </summary>
/// <remarks>The life stage is determined by the age of the animal in months, calculated from the provided date of
/// birth. The stages are categorized as follows: <list type="bullet"> <item><description><see
/// cref="AnimalLifeStage.Newborn"/>: Less than 1 month old.</description></item> <item><description><see
/// cref="AnimalLifeStage.Calf"/>: 1 to 5 months old.</description></item> <item><description><see
/// cref="AnimalLifeStage.Weaner"/>: 6 to 11 months old.</description></item> <item><description><see
/// cref="AnimalLifeStage.Heifer"/>: 12 to 23 months old.</description></item> <item><description><see
/// cref="AnimalLifeStage.Adult"/>: 24 months or older.</description></item> </list></remarks>
public sealed record LifeStage
{
    public LifeStage(DateTime dateOfBirth)
    {
        if (dateOfBirth > DateTime.UtcNow)
        {
            throw new ArgumentOutOfRangeException(nameof(dateOfBirth),
                "Date of birth cannot be in the future.");
        }
        int ageInMonths = (DateTime.UtcNow.Year - dateOfBirth.Year) * 12 + DateTime.UtcNow.Month - dateOfBirth.Month;
        AnimalLifeStage stage = ageInMonths switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(dateOfBirth),
                "Date of birth cannot be in the future."),
            >= 0 and < 1 => AnimalLifeStage.Newborn,
            < 6 => AnimalLifeStage.Calf,
            < 12 => AnimalLifeStage.Weaner,
            < 24 => AnimalLifeStage.Heifer,
            _ => AnimalLifeStage.Adult,
        };
        Value = stage;
        DateOfBirth = dateOfBirth;
    }
    public AnimalLifeStage Value { get; }
    public DateTime DateOfBirth { get; }
}
