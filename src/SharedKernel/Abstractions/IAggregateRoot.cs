namespace SharedKernel.Abstractions;

/// <summary>
/// Marker interface to indicate that a class is an aggregate root.
/// </summary>
/// <remarks>An aggregate root is the entry point to an aggregate, which is a cluster of domain objects that are
/// treated as a single unit. Implementing this interface signifies that the implementing class is responsible for
/// maintaining the consistency of the aggregate.</remarks>
public interface IAggregateRoot;
