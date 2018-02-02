using Backpacker.Ants;
using Backpacker.Cities;
using Backpacker.Database;

namespace Backpacker.Algorithms
{
    public interface IAlgorithm
    {
        IDatabase Database { get; set; }
        
        IAnt BestAnt { get; set; }
        
        void MoveToNextCity(IAnt ant);

        void AntDoneWalking(IAnt ant);
    }
}