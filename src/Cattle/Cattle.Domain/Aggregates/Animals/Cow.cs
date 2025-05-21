using Cattle.Domain.Aggregates.Herds;
using Cattle.Domain.Entities;
using Cattle.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace Cattle.Domain.Aggregates.Animals;

public class Cow : Animal, IAggregateRoot
{
    public Cow() : base() { } // For EF Core or any other ORM
    public Cow(
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
