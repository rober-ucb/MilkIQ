using Cattle.Domain.Aggregates.Animals;
using SharedKernel.Abstractions;

namespace Cattle.Domain.Aggregates.Herds;

public sealed class Herd : BaseEntity<int>, IAggregateRoot
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
    private readonly List<Animal> _cattle = [];
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ushort? MaximumNumberOfHerd { get; private set; }
    public IReadOnlyCollection<Animal> Cattle => _cattle.AsReadOnly();

    public static Herd Create(
        string name,
        string description,
        ushort capacity = 10)
    {
        return new Herd(name, description, capacity);
    }
    public void AddCattle(Animal animal)
    {
        if (_cattle.Count >= MaximumNumberOfHerd)
        {
            throw new InvalidOperationException("Cannot add more cattle to the herd.");
        }
        if (_cattle.Any(x => x.Id == animal.Id))
        {
            throw new InvalidOperationException("Animal already exists in the herd.");
        }
        _cattle.Add(animal);

    }
    public void RemoveCattle(Animal animal)
    {
        if (_cattle.Any(x => x.Id == animal.Id))
        {
            throw new InvalidOperationException("Animal not found in the herd.");
        }
        _cattle.Remove(animal);
    }
}
