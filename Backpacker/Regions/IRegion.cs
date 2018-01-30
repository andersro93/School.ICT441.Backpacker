using System.Collections.Concurrent;
using Backpacker.Cities;
using Backpacker.Countries;

namespace Backpacker.Regions
{
    public interface IRegion
    {
        string Name { get; set; }
        
        ICountry Country { get; set; }

        ConcurrentDictionary<string, ICity> Cities { get; set; }
    }
}