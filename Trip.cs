using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGBotNVK;

public class Trip
{
    public string From;
    public string To;
    public int ToPair;
    public int NumberOfAvailableSeats;
    public int Cost;
    public string CarPosition;
    public Driver driver;

    public Trip(Driver driver)
    {
        this.driver = driver;
    }

    public override string ToString()
    {
        return $"Из: {From} в: {To} цена: {Cost} руб";
    }

}
