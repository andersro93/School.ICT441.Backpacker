using System.Collections.Concurrent;
using Backpacker.Cities;
using Backpacker.Countries;
using Backpacker.Regions;

namespace Backpacker.Database
{
    public class Database : IDatabase
    {
        public ConcurrentDictionary<string, ICountry> Countries { get; set; }
        
        public ConcurrentBag<IRegion> Regions { get; set; }
        
        public ConcurrentBag<ICity> Cities { get; set; }

        public Database()
        {
            Countries = new ConcurrentDictionary<string, ICountry>();
            Regions = new ConcurrentBag<IRegion>();
            Cities = new ConcurrentBag<ICity>();
        }
        
        public ConcurrentBag<ICity> GetCities()
        {
            throw new System.NotImplementedException();
        }

        public ConcurrentBag<IRegion> GetRegions()
        {
            throw new System.NotImplementedException();
        }
    }
}