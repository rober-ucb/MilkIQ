using Cattle.Domain.Aggregates.Herds;
using Cattle.Domain.Entities;
using Cattle.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace Cattle.Domain.Aggregates.Animals;

public sealed class Cow : Animal, IAggregateRoot
{
    private readonly List<MilkingRecord> _milkingRecords = [];
    public Cow() : base() { } // For EF Core or any other ORM
    public Cow(
        Guid id,
        Herd herd,
        EarTag earTag,
        Breed breed,
        LifeStage lifeStage,
        bool isLactating,
        Weight? weight) : base(id, earTag, breed, lifeStage, herd, weight)
    {
        IsLactating = isLactating;
    }
    public IReadOnlyList<MilkingRecord> MilkingRecords => _milkingRecords;
    public bool IsLactating { get; private set; }
    public double AverageMilkYield => CalculateAverageMilkYield();
    public double TotalMilkYield => _milkingRecords.Sum(record => record.Volume.Amount);
    public MilkingRecord RecordMilkYield(MilkVolume volume)
    {
        if (!IsLactating)
        {
            throw new InvalidOperationException("Cow is not lactating.");
        }
        MilkingRecord milkingRecord = new(this, volume);
        _milkingRecords.Add(milkingRecord);
        return milkingRecord;
    }
    private double CalculateAverageMilkYield()
    {
        if (_milkingRecords.Count == 0)
        {
            return 0;
        }
        double totalVolume = _milkingRecords.Sum(record => record.Volume.Amount);
        return totalVolume / _milkingRecords.Count;
    }
}
