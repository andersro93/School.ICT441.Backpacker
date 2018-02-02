using System.Collections.Concurrent;
using Backpacker.Cities;
using Backpacker.Countries;
using Backpacker.Interfaces;

namespace Backpacker.Regions
{
    public class Region : IRegion
    {
        public string Name { get; set; }
        
        public ICountry Country { get; set; }

        public ConcurrentDictionary<string, ICity> Cities { get; set; }

        public Region()
        {
            Cities = new ConcurrentDictionary<string, ICity>();
        }
    }
}