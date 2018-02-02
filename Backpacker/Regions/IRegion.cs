using System.Collections.Concurrent;
using Backpacker.Cities;
using Backpacker.Countries;
using Backpacker.Interfaces;

namespace Backpacker.Regions
{
    public interface IRegion : IRouletteOption
    {
        string Name { get; set; }
        
        ICountry Country { get; set; }

        ConcurrentDictionary<string, ICity> Cities { get; set; }
    }
}