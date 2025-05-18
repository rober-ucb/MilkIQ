using Cattle.Domain.Aggregates.Herds;
using Cattle.Domain.Entities;
using SharedKernel.Abstractions;

namespace Cattle.Domain.Aggregates.Cattle;

public class Animal : BaseEntity<Guid>
{
    private const int EarTagLength = 5;
    internal protected Animal() { } // For EF Core or any other ORM
    internal protected Animal(
        Guid id,
        string earTag,
        Breed breed,
        LifeStage lifeStage,
        Herd? herd,
        Weight? weight)
    {
        Id = id;
        EarTag = earTag;
        BreedId = breed.Id;
        HerdId = herd?.Id;
        LifeStage = lifeStage;
        CurrentWeight = weight;
    }

    public static Animal Create(
        string earTag,
        Breed breed,
        LifeStage lifeStage,
        Herd? herd,
        Weight? weight = null
        )
    {
        if (earTag.Length is not EarTagLength)
        {
            throw new ArgumentException($"Ear tag must be {EarTagLength} characters long.", nameof(earTag));
        }
        return new Animal(Guid.NewGuid(), earTag, breed, lifeStage, herd, weight);
    }
    public string EarTag { get; private set; } = string.Empty;
    public int BreedId { get; private set; }
    public int? HerdId { get; private set; }
    public LifeStage LifeStage { get; private set; } = null!;
    public Weight? CurrentWeight { get; private set; }
    public Guid? SireId { get; private set; } // Father
    public Guid? DamId { get; private set; } // Mother
}
