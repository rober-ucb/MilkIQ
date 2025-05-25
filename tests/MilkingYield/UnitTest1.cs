using Xunit.Abstractions;

namespace MilkingYield;

public class UnitTest1(ITestOutputHelper outputHelper)
{
    [Fact]
    public void Test1()
    {
        TimeSpan timeOfDay = DateTime.UtcNow.TimeOfDay;
        string timeOfDayString = timeOfDay.ToString(@"hh\:mm\:ss");
        outputHelper.WriteLine(DateTime.UtcNow.TimeOfDay.Hours.ToString());
    }
}
