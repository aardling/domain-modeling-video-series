namespace Sharingo.Tests;

public interface IEvent { };
public interface ICommand { };

public record NoBikeReserved : IEvent
{

}

public record ReserveAnyBikeAtStation(string StationId) : ICommand
{

}

public record AnyBikeReserved(string StationId) : IEvent
{

}

public record BikeAddedToStation(string StationId) : IEvent
{
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
        return when switch
        {
            ReserveAnyBikeAtStation command => ReserveAnyBikeAtStation(command.StationId),
            _ => throw new Exception(),
        };
    }

    private IEvent ReserveAnyBikeAtStation(string stationId)
    {
        if (this.given.Count == 0)
        {
            return new NoBikeReserved();
        }

        return new AnyBikeReserved(stationId);
    }
}

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
        .When(new ReserveAnyBikeAtStation("S1"))
        .Then(new List<IEvent>() { new NoBikeReserved() });
    }

    [Test]
    public void Given_Bike_Added_To_Station_When_reserving_a_bike_Then_reservation_should_succeed()
    {
        new Scenario()
         .Given(new List<IEvent>() { new BikeAddedToStation("S1") })
         .When(new ReserveAnyBikeAtStation("S1"))
         .Then(new List<IEvent>() { new AnyBikeReserved("S1") });
    }

    [Test]
    public void Given_Bike_Added_To_Station_When_reserving_a_bike_at_different_station_Then_reservation_should_fail()
    {
        new Scenario()
         .Given(new List<IEvent>() { new BikeAddedToStation("S1") })
         .When(new ReserveAnyBikeAtStation("S2"))
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