using Milking.Domain.Entities;
using Milking.Domain.Enums;
using Milking.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace Milking.Domain.Aggregates;

public sealed class MilkingSession : BaseEntity<Guid>, IAggregateRoot
{
    private readonly List<MilkingRecord> _milkingRecords = [];
    private MilkingSession(
        Guid id,
        Guid employeeId,
        MilkingTime milkingTime)
    {
        Id = id;
        StartTime = DateTime.UtcNow;
        EmployeeId = employeeId;
        Status = MilkingSessionStatus.InProgress;
        MilkingTime = milkingTime;
    }
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public Guid EmployeeId { get; private set; }
    public double TotalYield => CalculateYield();
    public short TotalCowsMilked => (short)_milkingRecords.Count;
    public MilkingSessionStatus Status { get; private set; }
    public MilkingTime MilkingTime { get; private set; }
    public IReadOnlyList<MilkingRecord> MilkingRecords => _milkingRecords;
    public static MilkingSession StartSession(MilkingTime timeOfDay, Guid employeeId)
    {
        if (employeeId == Guid.Empty)
        {
            throw new ArgumentException("Employee ID cannot be empty.", nameof(employeeId));
        }

        return new(Guid.NewGuid(), employeeId, timeOfDay);
    }
    public MilkingRecord RecordMilking(Guid cowId, MilkVolume milkVolume)
    {
        if (Status is not MilkingSessionStatus.InProgress)
        {
            throw new InvalidOperationException("Cannot record milking for a session that is not in progress.");
        }
        MilkingRecord milkingRecord = new(cowId, milkVolume);
        _milkingRecords.Add(milkingRecord);
        return milkingRecord;
    }
    public void Complete()
    {
        if (Status is MilkingSessionStatus.Completed)
        {
            throw new InvalidOperationException("Cannot complete a session that is already completed.");
        }
        EndTime = DateTime.UtcNow;
        Status = MilkingSessionStatus.Completed;
    }
    public TimeSpan GetSessionDuration()
    {
        if (Status is not MilkingSessionStatus.Completed or MilkingSessionStatus.Cancelled)
        {
            throw new InvalidOperationException("Session is still in progress.");
        }
        return (EndTime ?? DateTime.UtcNow) - StartTime;
    }
    private double CalculateYield() => _milkingRecords.Sum(x => x.Volume.Amount);
}
public enum MilkingTime
{
    Morning,
    Afternoon,
}