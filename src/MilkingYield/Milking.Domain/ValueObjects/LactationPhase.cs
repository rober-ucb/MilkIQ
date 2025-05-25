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
        (LactationStage currentPhase, DateTime startDate) = DeterminePhaseBy(daysInMilk);
        
        if (StartDate > DateTime.UtcNow)
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
    private static (LactationStage, DateTime) DeterminePhaseBy(short daysInMilk) 
        => daysInMilk switch
    {
        >= 0 and <= 30 => (LactationStage.Fresh, DateTime.UtcNow.AddDays(60)),
        >= 31 and <= 90 => (LactationStage.Peak, DateTime.UtcNow.AddDays(90)),
        >= 91 and <= 150 => (LactationStage.Mid, DateTime.UtcNow.AddDays(150)),
        >= 151 and <= 290 => (LactationStage.Late, DateTime.UtcNow.AddDays(290)),
        _ => (LactationStage.Dry, DateTime.UtcNow.AddDays(365)),
    };
    public bool IsInPeakProduction => CurrentPhase == LactationStage.Peak;
    public bool IsInLactation => CurrentPhase != LactationStage.Dry;
    public LactationStage CurrentPhase { get; init; }
    public short DaysInMilk { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime CalvingDate { get; init; }

}
