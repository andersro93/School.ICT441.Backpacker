using System.Collections.Concurrent;
using Backpacker.Interfaces;
using Backpacker.Regions;

namespace Backpacker.Countries
{
    public class Country : ICountry
    {
        public string Name { get; set; }
        
        public ConcurrentDictionary<string, IRegion> Regions { get; set; }

        public Country()
        {
            Regions = new ConcurrentDictionary<string, IRegion>();
        }
    }
}