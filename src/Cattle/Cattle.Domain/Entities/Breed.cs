using SharedKernel.Abstractions;

namespace Cattle.Domain.Entities;

public class Breed : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
}
