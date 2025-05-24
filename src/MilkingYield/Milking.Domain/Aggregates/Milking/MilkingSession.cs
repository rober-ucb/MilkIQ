using Cattle.Domain.Entities;
using Cattle.Domain.Enums;
using SharedKernel.Abstractions;

namespace Cattle.Domain.Aggregates.Milking;

public sealed class MilkingSession : BaseEntity<Guid>, IAggregateRoot
{
    private MilkingSession() { }
    private MilkingSession(
        Employee employee,
        VolumeUnit unit)
    {
        EmployeeId = employee.Id;
        Status = MilkingSessionStatus.InProgress;
        VolumeUnit = unit;
        StartTime = DateTime.UtcNow;
    }
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public MilkingSessionStatus Status { get; private set; }
    public string EmployeeId { get; private set; } = string.Empty;
    public ushort NumberOfCowsToMilk { get; private set; }
    public VolumeUnit VolumeUnit { get; private set; }
    public static MilkingSession StartMilkSession(Employee employee, VolumeUnit unit) => new(employee, unit);
    public void CompleteSession()
    {
        EndTime = DateTime.UtcNow;
        Status = MilkingSessionStatus.Completed;
    }
    public void CancelSession()
    {
        if (Status == MilkingSessionStatus.Completed)
        {
            throw new InvalidOperationException("Cannot cancel a completed session.");
        }
        EndTime = DateTime.UtcNow;
        Status = MilkingSessionStatus.Cancelled;
    }
    public TimeSpan GetSessionDuration()
    {
        if (Status is not MilkingSessionStatus.Completed or MilkingSessionStatus.Cancelled)
        {
            throw new InvalidOperationException("Session is still in progress.");
        }
        return (EndTime ?? DateTime.UtcNow) - StartTime;
    }
}