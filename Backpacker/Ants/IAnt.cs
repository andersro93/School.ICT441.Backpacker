using System.Collections.Generic;
using Backpacker.Algorithms;
using Backpacker.Cities;
using Backpacker.Colony;

namespace Backpacker.Ants
{
    public interface IAnt
    {
        string Name { get; set; }
        
        int CitiesToVisit { get; set; }

        ICity Current { get; set; }

        ICity Destination { get; set; }

        IAlgorithm Algorithm { get; set; }
        
        IColony Colony { get; set; }

        List<ICity> Visited { get; set; }

        void Walk();
    }
}