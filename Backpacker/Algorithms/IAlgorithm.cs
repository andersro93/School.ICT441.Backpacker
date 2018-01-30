using Backpacker.Ants;
using Backpacker.Cities;
using Backpacker.Database;

namespace Backpacker.Algorithms
{
    public interface IAlgorithm
    {
        IDatabase Database { get; set; }
        
        ICity GetNextCity(IAnt ant);
    }
}