using System;
using System.Linq;
using Backpacker.Algorithms;
using Backpacker.Ants;
using Backpacker.Cities;
using Backpacker.Colony;
using Backpacker.Countries;
using Backpacker.Database;
using Backpacker.Parser;
using Backpacker.Regions;

namespace Backpacker
{
    class Program
    {
        private static int CitiesToVisit = 5;
        
        private static int Ants = 1000000;
        
        static void Main(string[] args)
        {
            WorldCitiesParser parser = new WorldCitiesParser();

            Console.WriteLine("MultiThreaded Parser - Starting");
            IDatabase database = parser.ParseWorldCitiesPopDb("./Data/worldcitiespop.txt");
            Console.WriteLine("MultiThreaded Parser - Completed");
            
            Console.WriteLine($"----------------------------------------");
            Console.WriteLine($"Stats: ");
            Console.WriteLine($"Countries: {database.Countries.Count}");
            Console.WriteLine($"Regions: {database.Regions.Count}");
            Console.WriteLine($"Cities: {database.Cities.Count}");
            
            Console.WriteLine($"----------------------------------------");
            Console.WriteLine($"Trip info: ");
            ICity startCity = database.Cities.First(city => city.Name == "oslo");
            Console.WriteLine($"Start City: {startCity.Name}");

            ICity endCity = database.Cities.First(city => city.Name == "london");
            Console.WriteLine($"Destination City: {endCity.Name}");
            Console.WriteLine($"Cities to visit in total: {CitiesToVisit}");
            
            Console.WriteLine($"----------------------------------------");
            Console.WriteLine($"Creating colony with {Ants} ants");
            IColony colony = new Colony.Colony();
            
            MMAS algorithm = new MMAS();
            algorithm.Database = database;
            colony.Algorithm = algorithm;
            
            colony.Start = startCity;
            colony.Destination = endCity;
            colony.CitiesToVisit = CitiesToVisit;
            colony.CreateAnts(Ants);

            Console.WriteLine($"Colony created!");
            
            Console.WriteLine($"----------------------------------------");
            Console.WriteLine($"Starting the walk");

            IAnt bestAnt = colony.TraverseUntilConvergence();
            
            Console.WriteLine($"Walk done");
            Console.WriteLine($"----------------------------------------");
            Console.WriteLine($"Best ant {bestAnt.Name}");
        }
    }
}