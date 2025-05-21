using Cattle.Domain.Aggregates.Animals;
using Cattle.Domain.Entities;
using Cattle.Domain.Enums;
using Cattle.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace Cattle.Domain.Aggregates.Milking;

public sealed class MilkingSession : BaseEntity<int>, IAggregateRoot
{
    private readonly List<Cow> _cows = [];
    private readonly List<MilkingRecord> _milkingRecords = [];
    private readonly VolumeUnit _volumeUnit = VolumeUnit.Liter;
    private MilkingSession() { }
    private MilkingSession(
        DateTime startTime,
        DateTime? endTime,
        List<Cow> cowsToMilk,
        Operator user,
        VolumeUnit unit)
    {
        StartTime = startTime;
        EndTime = endTime;
        _cows = cowsToMilk;
        OperatorId = user.Id;
        NumberOfCowsToMilk = (ushort)cowsToMilk.Count;
        Status = MilkingSessionStatus.InProgress;
        _volumeUnit = unit;
    }
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public MilkingSessionStatus Status { get; private set; }
    public string OperatorId { get; private set; } = string.Empty;
    public ushort NumberOfCowsToMilk { get; private set; }
    public MilkVolume TotalCollectedVolume => CalculateTotalMilkVolume();
    public IReadOnlyCollection<Cow> Cows => _cows;
    public IReadOnlyCollection<MilkingRecord> MilkingRecords => _milkingRecords;
    public static MilkingSession StartMilkSession(
        DateTime startTime,
        List<Cow> cows,
        Operator user,
        VolumeUnit unit
        )
    {
        if (cows.Count == 0)
        {
            throw new ArgumentException("No cows to milk.", nameof(cows));
        }
        if (cows.Any(cow => !cow.IsLactating))
        {
            throw new ArgumentException("All cows must be lactating.", nameof(cows));
        }
        return new MilkingSession(startTime, null, cows, user, unit);
    }
    public void CompleteSession()
    {
        if (_cows.Count > 0)
        {
            throw new InvalidOperationException($"Cows have not been milked yet");
        }

        _cows.Clear();
        EndTime = DateTime.UtcNow;
        Status = MilkingSessionStatus.Completed;
    }
    public MilkingRecord RecordMilkCollection(Cow cow, MilkVolume volume)
    {
        if (Status is not MilkingSessionStatus.InProgress)
        {
            throw new InvalidOperationException("Milking session is not in progress.");
        }
        if (!_cows.Contains(cow))
        {
            throw new InvalidOperationException("Cow is not part of this milking session.");
        }
        MilkingRecord milkingRecord = new(cow, volume);
        _milkingRecords.Add(milkingRecord);
        _cows.Remove(cow);
        return milkingRecord;
    }
    public void CancelSession()
    {
        if (Status == MilkingSessionStatus.Completed)
        {
            throw new InvalidOperationException("Cannot cancel a completed session.");
        }
        _cows.Clear();
        EndTime = DateTime.UtcNow;
        Status = MilkingSessionStatus.Cancelled;
    }
    public TimeSpan GetSessionDuration()
    {
        if (Status is not MilkingSessionStatus.Completed or MilkingSessionStatus.Cancelled)
        {
            throw new InvalidOperationException("Session is still in progress.");
        }
        return (EndTime ?? DateTime.UtcNow) - StartTime;
    }
    public double GetAverageMilkVolumePerCow() => CalculateTotalMilkVolume().Amount / _cows.Count;
    private MilkVolume CalculateTotalMilkVolume()
    {
        if (_milkingRecords.Count == 0)
        {
            return new MilkVolume(0, _volumeUnit);
        }
        double totalAmount = _milkingRecords.Sum(record => record.Volume.Amount);
        return new MilkVolume(totalAmount, _volumeUnit);
    }
}