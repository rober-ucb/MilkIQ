namespace Milking.Domain.ValueObjects;

// I want to create a value object that represents the time of day when milking occurs.
// It should be immutable and should validate that the time is within a valid range (e.g., 00:00 to 23:59).
// It should also have the name of time day, for example "Morning", "Afternoon", "Noon"
public sealed record MilkingTime
{
    public MilkingTime(TimeSpan timeOfDay)
    {

    }
}
