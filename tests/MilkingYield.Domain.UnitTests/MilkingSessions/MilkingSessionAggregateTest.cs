using Milking.Domain.Aggregates;

namespace MilkingYield.Domain.UnitTests.MilkingSessions;

public class MilkingSessionAggregateTest
{
    // Convention name: [ThingUnderTest}_Should_[ExpectedResult]_[Conditions]
    [Fact]
    public void MilkingSession_Should_ThrowException_When_WhenMilkingStatusIsInProgress()
    {
        // Arrange
        MilkingSession milkingSession = MilkingSession.StartSession(MilkingTime.Morning, Guid.NewGuid());
    }
}
