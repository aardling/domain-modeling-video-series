namespace Sharingo.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SanityTest()
    {
        Assert.Pass();
    }

    [Test]
    public void Given_no_bikes_When_reserving_a_bike_Then_reservation_should_fail()
    {
        new Scenario()
        .Given(new List<IEvent>())
        .When(new ReserveAnyBikeAtStation())
        .Then(new List<IEvent>() { new NoBikeReserved() });
    }
}

public class Scenario
{
    public Scenario When(ReserveAnyBikeAtStation reserveAnyBikeAtStation)
    {
        return this;
    }

    public Scenario Given(List<IEvent> events)
    {
        return this;
    }

    public void Then(List<IEvent> events)
    {
        Assert.Fail();
    }
}

public interface IEvent
{

}

public record NoBikeReserved : IEvent
{

}

public record ReserveAnyBikeAtStation
{

}