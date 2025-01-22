using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGBotNVK;

public class Driver
{
    //private Car car;
    public Car car { get; private set; }
    private int rating;
    private int allTripsCount;

    public List<Trip> trips = new List<Trip>();
    public Trip CreatedTrip;
    public long TGId { get; private set; }

    public bool IsProfileComplete => car.Name != "" && car.Number != "" && car.Color != "" && TGId != null;

    public Driver(long id) 
    {
        car = new();
        TGId = id;
    }

    public void SetAutoName(string name)
    {
        car.Name = name;
    }

    public void SetAutoNumber(string number)
    {
        car.Number = number;
    }

    public void SetAutoColor(string color)
    {
        car.Color = color;
    }

    public void SetTGId(long id)
    {
        TGId = id;
    }

    public override string ToString()
    {
        return $"{car}, tg id: {TGId}";
    }

    public string GetAutoInfo()
    {
        return car.ToString();
    }

    public void EndCreatingTrip()
    {
        trips.Add(CreatedTrip);
        CreatedTrip = new(this);
    }

    public string GetTrips()
    {
        var result = new StringBuilder();

        foreach (var trip in trips)
        {
            result.Append(trip.ToString());
        }

        return result.ToString();
    }
}
