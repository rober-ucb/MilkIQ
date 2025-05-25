using Milking.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace Milking.Domain.Entities;

public class MilkingRecord : BaseEntity<int>
{
    internal MilkingRecord(Guid cowId, MilkVolume volume)
    {
        CowId = cowId;
        Volume = volume;
        RecordedAt = DateTime.UtcNow;
    }
    public Guid CowId { get; private set; }
    public MilkVolume Volume { get; private set; } = null!;
    public DateTime RecordedAt { get; private set; }
}
