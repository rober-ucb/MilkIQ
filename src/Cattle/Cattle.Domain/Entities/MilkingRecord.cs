using Cattle.Domain.Aggregates.Animals;
using Cattle.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace Cattle.Domain.Entities;

public class MilkingRecord : BaseEntity<int>
{
    private MilkingRecord() { } // For EF Core or any other ORM
    internal MilkingRecord(Cow cow, MilkVolume volume)
    {
        CowId = cow.Id;
        Volume = volume;
        RecordedAt = DateTime.UtcNow;
    }
    public Guid CowId { get; private set; }
    public MilkVolume Volume { get; private set; } = null!;
    public DateTime RecordedAt { get; private set; }
}
