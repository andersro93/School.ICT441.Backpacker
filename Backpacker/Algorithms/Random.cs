using Backpacker.Ants;
using Backpacker.Cities;
using Backpacker.Database;

namespace Backpacker.Algorithms
{
    public class Random : IAlgorithm
    {
        public IDatabase Database { get; set; }

        public ICity GetNextCity(IAnt ant)
        {
            throw new System.NotImplementedException();
        }
    }
}