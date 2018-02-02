using System;
using System.Collections.Generic;
using Backpacker.Algorithms;
using Backpacker.Cities;
using Backpacker.Colony;

namespace Backpacker.Ants
{
    public class Ant : IAnt
    {
        public string Name { get; set; }
        
        public int CitiesToVisit { get; set; }

        public ICity Current { get; set; }
        
        public ICity Destination { get; set; }
        
        public IAlgorithm Algorithm { get; set; }
        
        public IColony Colony { get; set; }

        public List<ICity> Visited { get; set; }

        public Ant()
        {
            Visited = new List<ICity>();
        }

        public void Prepare()
        {
            
        }

        public void Walk()
        {           
            Algorithm.MoveToNextCity(this);
        }

        public void Done()
        {
            Algorithm.AntDoneWalking(this);
        }
    }
}