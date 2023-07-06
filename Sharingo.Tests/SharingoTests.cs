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
    private List<IEvent> _given;
    private ICommand _when;

    public Scenario When(ICommand when)
    {
        this._when = when;
        return this;
    }

    public Scenario Given(List<IEvent> given)
    {
        this._given = given;
        return this;
    }

    public void Then(List<IEvent> then)
    {
        var actualEvent = new BikeAvailability(this._given).Handle(this._when);
        Assert.That(then, Is.EqualTo(new List<IEvent> { actualEvent }));
    }
}

class BikeAvailability
{
    private List<IEvent> given;

    public BikeAvailability(List<IEvent> given)
    {
        this.given = given;
    }

    public IEvent Handle(ICommand when)
    {
        return new NoBikeReserved();
    }
}

public interface IEvent { };
public interface ICommand { };

public record NoBikeReserved : IEvent
{

}

public record ReserveAnyBikeAtStation : ICommand
{

}