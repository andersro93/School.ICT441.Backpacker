using System.Collections.Concurrent;
using Backpacker.Cities;
using Backpacker.Countries;
using Backpacker.Regions;

namespace Backpacker.Database
{
    public interface IDatabase
    {
        ConcurrentDictionary<string, ICountry> Countries { get; set; }
        
        ConcurrentBag<IRegion> Regions { get; set; }
        
        ConcurrentBag<ICity> Cities { get; set; }

        ConcurrentBag<ICity> GetCities();
        
        ConcurrentBag<IRegion> GetRegions();
    }
}