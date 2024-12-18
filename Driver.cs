using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGBotNVK;

public class Driver
{
    private string autoName;
    private string autoNumber;
    private string autoColor;
    private long TGId;
    private int rating;
    private int allTripsCount;

    public List<Trip> trips = new List<Trip>();
    public Trip CreatedTrip;

    public bool IsProfileComplete => autoName != null && autoNumber != null && autoColor != null && TGId != null;

    public Driver() { }

    public void SetAutoName(string name)
    {
        autoName = name;
    }

    public void SetAutoNumber(string number)
    {
        autoNumber = number;
    }

    public void SetAutoColor(string color) 
    { 
        autoColor = color;
    }

    public void SetTGId(long id) 
    {
        TGId = id;
    }

    public override string ToString()
    {
        return $"Автомобиль: {autoName}, номер: {autoNumber}, цвет: {autoColor}, tg id: {TGId}";
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
