using Cattle.Domain.Aggregates.Herds;
using Cattle.Domain.Entities;
using Cattle.Domain.Enums;
using Cattle.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace Cattle.Domain.Aggregates.Animals;

public class Animal : BaseEntity<Guid>
{
    private readonly List<Weight> _weightHistory = [];
    internal protected Animal() { } // For EF Core or any other ORM
    internal protected Animal(
        Guid id,
        EarTag earTag,
        Breed breed,
        LifeStage lifeStage,
        Herd? herd,
        Weight? weight)
    {
        Id = id;
        EarTag = earTag;
        BreedId = breed.Id;
        HerdId = herd?.Id;
        Stage = lifeStage;
        CurrentWeight = weight;
    }

    public static Animal Create(
        EarTag earTag,
        Breed breed,
        DateTime dateOfBirth,
        Herd? herd,
        double? weight
        )
    {
        LifeStage lifeStage = new(dateOfBirth);
        Weight? currentWeight = weight is null ? null : new Weight(weight.Value, WeightUnit.Kg);
        return new Animal(Guid.NewGuid(), earTag, breed, lifeStage, herd, currentWeight);
    }
    public EarTag EarTag { get; private set; } = null!;
    public int BreedId { get; private set; }
    public Guid? HerdId { get; private set; }
    public LifeStage Stage { get; private set; } = null!;
    public Weight? CurrentWeight { get; private set; }
    public Guid? SireId { get; private set; } // Father
    public Guid? DamId { get; private set; } // Mother
    public IReadOnlyList<Weight> WeightHistory => _weightHistory;

    public void UpdateWeight(Weight weight)
    {
        _weightHistory.Add(weight);
        CurrentWeight = weight;
    }
    public void SetParents(Guid? sireId, Guid? damId)
    {
        if (sireId is null && damId is null)
        {
            throw new ArgumentException("At least one parent must be specified.");
        }
        SireId = sireId;
        DamId = damId;
    }
}
