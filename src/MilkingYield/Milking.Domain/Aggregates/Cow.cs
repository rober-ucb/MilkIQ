using Milking.Domain.Enums;
using Milking.Domain.ValueObjects;
using SharedKernel.Abstractions;

namespace Milking.Domain.Aggregates;

public sealed class Cow : BaseEntity<Guid>, IAggregateRoot
{
    private Cow(Guid id, string name, string tagNumber, LactationPhase lactation)
    {
        Id = id;
        Name = name;
        TagNumber = tagNumber;
        LactationPhase = lactation;
    }
    public static Cow Create(string name, string tagNumber, LactationPhase lactation) 
        => new(Guid.NewGuid(), name, tagNumber, lactation);
    public string Name { get; private set; } = string.Empty;
    public string TagNumber { get; private set; } = string.Empty;
    public HealthStatus HealthStatus { get; private set; }
    public LactationPhase LactationPhase { get; private set; }
    public void StartLactationPhase()
    {
        if (LactationPhase.CurrentPhase != LactationStage.Dry)
        {
            throw new InvalidOperationException("Cannot start lactation phase when not in dry stage.");
        }
        LactationPhase newPhase = new(DateTime.UtcNow);
        LactationPhase = newPhase;
    }
    public void DryOff()
    {
        LactationPhase = LactationPhase with
        {
            CurrentPhase = LactationStage.Dry
        };
    }
}
