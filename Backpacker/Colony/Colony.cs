using System.Collections.Concurrent;
using System.Threading.Tasks;
using Backpacker.Algorithms;
using Backpacker.Ants;
using Backpacker.Cities;

namespace Backpacker.Colony
{
    public class Colony : IColony
    {
        public ICity Start { get; set; }
        
        public ICity Destination { get; set; }
        
        public IAlgorithm Algorithm { get; set; }
        
        public IAnt BestAnt { get; set; }
        
        public int CitiesToVisit { get; set; }

        public ConcurrentBag<IAnt> Ants { get; set; }
        
        protected ParallelOptions _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 25
        };
        
        public IColony CreateAnts(int amount)
        {
            Parallel.For(1, amount + 1, _parallelOptions, x =>
            {
                Ants.Add(CreateAnt($"Ant: {x}"));
            });

            return this;
        }

        public Colony()
        {
            Ants = new ConcurrentBag<IAnt>();
        }

        protected IAnt CreateAnt(string Name)
        {
            return new Ant
            {
	    	Name = Name,
                Algorithm = Algorithm,
                Colony = this,
                Current = Start,
                Destination = Destination,
                CitiesToVisit = CitiesToVisit
            };
        }
    }
}
