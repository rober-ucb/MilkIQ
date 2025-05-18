using Cattle.Domain.Aggregates.Herds;
using Cattle.Domain.Entities;
using SharedKernel.Abstractions;

namespace Cattle.Domain.Aggregates.Cattle;

public class Cow : Animal, IAggregateRoot
{
    private Cow() : base() { } // For EF Core or any other ORM
    private Cow(
        Guid id,
        Herd herd,
        string earTag,
        Breed breed,
        LifeStage lifeStage,
        bool isPregnant,
        bool isLactating,
        Weight? weight) : base(id, earTag, breed, lifeStage, herd, weight)
    {
        IsPregnant = isPregnant;
        IsLactating = isLactating;
    }
    public bool IsPregnant { get; private set; }
    public bool IsLactating { get; private set; }
    public void StartPhaseOfLactation(DateTime startDate)
    {
        if (IsLactating)
        {
            throw new InvalidOperationException("Cow is already lactating.");
        }
        IsLactating = true;
    }
}
