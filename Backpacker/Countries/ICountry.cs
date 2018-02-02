using System.Collections.Concurrent;
using Backpacker.Interfaces;
using Backpacker.Regions;

namespace Backpacker.Countries
{
    public interface ICountry : IRouletteOption
    {
        string Name { get; set; }
        
        ConcurrentDictionary<string, IRegion> Regions { get; set; }
    }
}