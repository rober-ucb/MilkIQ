using Milking.Domain.Enums;

namespace Milking.Domain.ValueObjects;

public sealed record LactationPhase
{
    public LactationPhase(DateTime calvingDate)
    {
        short daysInMilk = (short)(DateTime.UtcNow - calvingDate).TotalDays;
        if (daysInMilk < 0)
        {
            throw new ArgumentException("Calving date cannot be in the future.");
        }
        LactationStage currentPhase = DeterminePhaseBy(daysInMilk);
        DateTime startDate = currentPhase switch
        {
            LactationStage.Fresh => calvingDate,
            LactationStage.Peak => calvingDate.AddMonths(3),
            LactationStage.Mid => calvingDate.AddMonths(5),
            LactationStage.Late => calvingDate.AddDays(6),
            LactationStage.Dry => calvingDate.AddDays(7),
            _ => calvingDate
        };
        if (startDate > DateTime.UtcNow)
        {
            throw new ArgumentException("Start date cannot be in the future.");
        }
        if (startDate < calvingDate)
        {
            throw new ArgumentException("Start date cannot be before calving date.");
        }
        DaysInMilk = daysInMilk;
        CurrentPhase = currentPhase;
        StartDate = startDate;
        CalvingDate = calvingDate;
    }
    private static LactationStage DeterminePhaseBy(short daysInMilk) => daysInMilk switch
    {
        >= 0 and <= 30 => LactationStage.Fresh,
        >= 31 and <= 100 => LactationStage.Peak,
        _ => LactationStage.Dry
    };
    public bool IsInPeakProduction => CurrentPhase == LactationStage.Peak;
    public bool IsInLactation => CurrentPhase != LactationStage.Dry;
    public LactationStage CurrentPhase { get; init; }
    public short DaysInMilk { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime CalvingDate { get; init; }

}
