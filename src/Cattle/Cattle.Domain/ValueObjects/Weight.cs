using Cattle.Domain.Enums;

namespace Cattle.Domain.ValueObjects;

/// <summary>
/// Represents a weight measurement with a specified value and unit.
/// </summary>
/// <remarks>This type is immutable and provides a way to encapsulate a weight value along with its associated
/// unit. Use this type to ensure consistency when working with weight-related data.</remarks>
public sealed record Weight
{
    public Weight(double value, WeightUnit unit)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Weight cannot be negative.");
        }
        Value = value;
        Unit = unit;
    }
    public double Value { get; }
    public WeightUnit Unit { get; }
}
