using Cattle.Domain.Enums;

namespace Cattle.Domain.ValueObjects;

public record MilkVolume
{
    public MilkVolume(double amount, VolumeUnit unit)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Volume cannot be negative.");
        }
        Amount = amount;
        Unit = unit;
    }
    public double Amount { get; init; }
    public VolumeUnit Unit { get; init; }
}
