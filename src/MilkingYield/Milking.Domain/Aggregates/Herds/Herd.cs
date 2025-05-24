using Cattle.Domain.Aggregates.Animals;
using SharedKernel.Abstractions;

namespace Cattle.Domain.Aggregates.Herds;

public sealed class Herd : BaseEntity<Guid>, IAggregateRoot
{
    private Herd() { } // For EF Core or any other ORM
    private Herd(
        string name,
        string description,
        ushort capacity)
    {
        Name = name;
        Description = description;
        MaximumNumberOfHerd = capacity;
    }
    private readonly List<Guid> _cattle = [];
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ushort? MaximumNumberOfHerd { get; private set; }
    public IReadOnlyCollection<Guid> Cattle => _cattle;

    public static Herd Create(
        string name,
        string description,
        ushort capacity = 20)
    {
        return new Herd(name, description, capacity);
    }
    public void AddCattle(Animal animal)
    {
        if (_cattle.Count >= MaximumNumberOfHerd)
        {
            throw new InvalidOperationException("Cannot add more cattle to the herd.");
        }
        if (_cattle.Any(id => id == animal.Id))
        {
            throw new InvalidOperationException("Animal already exists in the herd.");
        }
        _cattle.Add(animal.Id);

    }
    public void RemoveCattle(Animal animal)
    {
        if (_cattle.Any(id => id == animal.Id))
        {
            throw new InvalidOperationException("Animal not found in the herd.");
        }
        _cattle.Remove(animal.Id);
    }
}
