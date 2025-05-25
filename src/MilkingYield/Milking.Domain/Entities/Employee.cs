using SharedKernel.Abstractions;

namespace Cattle.Domain.Entities;

public class Employee : BaseEntity<string>
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set;} = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

}
