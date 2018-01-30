using System.Collections.Concurrent;
using Backpacker.Regions;

namespace Backpacker.Countries
{
    public interface ICountry
    {
        string Name { get; set; }
        
        ConcurrentDictionary<string, IRegion> Regions { get; set; }
    }
}