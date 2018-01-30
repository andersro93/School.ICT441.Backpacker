using System.Collections.Concurrent;
using Backpacker.Algorithms;
using Backpacker.Ants;
using Backpacker.Cities;

namespace Backpacker.Colony
{
    public interface IColony
    {
        ICity Start { get; set; }
        
        ICity Destination { get; set; }
        
        IAlgorithm Algorithm { get; set; }
        
        IAnt BestAnt { get; set; }
        
        int CitiesToVisit { get; set; }
        
        ConcurrentBag<IAnt> Ants { get; set; }

        IColony CreateAnts(int amount);
    }
}