using Milking.Domain.Enums;
using Milking.Domain.ValueObjects;
using Xunit.Abstractions;

namespace MilkingYield.Domain.UnitTests.ValueObjects;

public class LactationPhaseTests(ITestOutputHelper outputHelper)
{
    //[ThingUnderTest}_Should_[ExpectedResult]_[Conditions]
    [Theory]
    [InlineData(10)]
    [InlineData(3)]
    [InlineData(30)]
    public void LactationPhase_Should_ThrowException_When_CalvingDateIsFuture(short days)
    {
        DateTime calvingDate = DateTime.UtcNow.AddDays(days);
        outputHelper.WriteLine($"CalvingDate: {calvingDate}");
        Assert.Throws<ArgumentException>(() => new LactationPhase(calvingDate));
    }

    [Theory]
    [InlineData(30, LactationStage.Fresh)]
    [InlineData(1, LactationStage.Fresh)]
    [InlineData(0, LactationStage.Fresh)]
    [InlineData(31, LactationStage.Peak)]
    [InlineData(90, LactationStage.Peak)]
    [InlineData(89, LactationStage.Peak)]
    public void DeterminePhaseBy_Should_ReturnFresh_When_DaysInMilkIsLessThan60(
        short daysInMilk, LactationStage phaseExpected)
    {
        DateTime calvingDate = DateTime.UtcNow.AddDays(-daysInMilk);
        outputHelper.WriteLine($"Calving Date: {calvingDate}");
        
        LactationPhase lactationPhase = new(calvingDate);
        outputHelper.WriteLine($"Current Phase: {lactationPhase.CurrentPhase}");

        Assert.Equal(phaseExpected, lactationPhase.CurrentPhase);
    }

    [Theory]
    [InlineData(61, LactationStage.Mid)]
    [InlineData(150, LactationStage.Mid)]
    [InlineData(200, LactationStage.Late)]
    public void DeterminePhaseBy_Should_ReturnLateLactation_When_DaysInMilkIsGreaterThan61(
        short daysInMilk, LactationStage phaseExpected)
    {
        DateTime calvingDate = DateTime.UtcNow.AddDays(-daysInMilk);
        outputHelper.WriteLine($"Calving Date: {calvingDate}");

        LactationPhase lactationPhase = new(calvingDate);
        outputHelper.WriteLine($"Current Phase: {lactationPhase.CurrentPhase}");

        Assert.Equal(phaseExpected, lactationPhase.CurrentPhase);
    }

    [Theory]
    [InlineData(60)]
    [InlineData(90)]
    [InlineData(89)]
    [InlineData(31)]
    public void IsInPeakProduction_Should_ReturnTrue_When_InPeakProduction(short days)
    {
        DateTime calvingDate = DateTime.UtcNow.AddDays(-days);
        LactationPhase lactationPhase = new(calvingDate);
        outputHelper.WriteLine($"Current Phase: {lactationPhase.CurrentPhase}");
        Assert.True(lactationPhase.IsInPeakProduction);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(60)]
    [InlineData(90)]
    [InlineData(150)]
    [InlineData(200)]
    [InlineData(210)]
    public void IsInLactation_Should_ReturnTrue_When_InLactation(short days)
    {
        DateTime calvingDate = DateTime.UtcNow.AddDays(-days);
        LactationPhase lactationPhase = new(calvingDate);
        outputHelper.WriteLine($"Current Phase: {lactationPhase.CurrentPhase}");
        Assert.True(lactationPhase.IsInLactation);
    }

    [Theory]
    [InlineData(211)]
    public void IsInLactation_Should_ReturnFalse_When_InDryPeriod(short days)
    {
        DateTime calvingDate = DateTime.UtcNow.AddDays(-days);
        LactationPhase lactationPhase = new(calvingDate);
        outputHelper.WriteLine($"Current Phase: {lactationPhase.CurrentPhase}");
        Assert.False(lactationPhase.IsInLactation);
    }

    [Fact]
    public void Test_Anything_You_Want()
    {
        outputHelper.WriteLine($"{DateTime.UtcNow.AddMonths(3)}");
    }
}
