namespace SharedKernel.Abstractions;

/// <summary>
/// Represents a base class for entities with a unique identifier of type <typeparamref name="TId"/>.
/// </summary>
/// <remarks>This class provides a foundation for entity types by enforcing the presence of a unique identifier
/// and implementing equality comparison based on the identifier. It also includes operator overloads for equality and
/// inequality checks.</remarks>
/// <typeparam name="TId">The type of the unique identifier for the entity. Must be non-nullable and implement  <see cref="IEquatable{T}"/>
/// and <see cref="IComparable{T}"/>.</typeparam>
public abstract class BaseEntity<TId> : IEquatable<BaseEntity<TId>>
    where TId : notnull, IEquatable<TId>, IComparable<TId>
{
    public TId Id { get; protected set; } = default!;
    public override bool Equals(object? obj) => Equals(obj as BaseEntity<TId>);
    public bool Equals(BaseEntity<TId>? other) => other is not null && Id.Equals(other.Id);
    public override int GetHashCode() => Id.GetHashCode() * 41;
    public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right) => Equals(left, right);
    public static bool operator !=(BaseEntity<TId>? left, BaseEntity<TId>? right) => !Equals(left, right);
    public void PublishEvent()
    {
        Console.WriteLine("Event published.");
    }
}