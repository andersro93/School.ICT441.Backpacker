using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backpacker.Cities;
using Backpacker.Countries;
using Backpacker.Database;
using Backpacker.Regions;

namespace Backpacker.Parser
{
    public class WorldCitiesParser
    {
        protected const string CsvDelimeter = ",";
        
        protected const int Threads = 25;
        
        protected const int FieldCountryName = 0;

        protected const int FieldCityName = 1;
        
        protected const int FieldRegionName = 3;
        
        protected const int FieldCityPopulation = 4;
        
        protected const int FieldCityLatitude = 5;
        
        protected const int FieldCityLongitude = 6;

        protected IDatabase Database;

        private ParallelOptions _parallelOptions;

        public WorldCitiesParser()
        {
            _parallelOptions = new ParallelOptions();
            _parallelOptions.MaxDegreeOfParallelism = Threads;
        }

        public IDatabase ParseWorldCitiesPopDb(string path)
        {
            ConcurrentBag<string[]> Lines = new ConcurrentBag<string[]>();
            
            Console.WriteLine("Starting file read to memory");
            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    Lines.Add(reader.ReadLine().Split(CsvDelimeter));
                }
            }
            Console.WriteLine("Done File read to memory");
            
            Database = new Database.Database();

            Console.WriteLine("Starting parsing lines to database");
            Parallel.ForEach(Lines, _parallelOptions, line => AddLineToDatabase(line));
            Console.WriteLine("Dome parsing lines to database");

            Console.WriteLine("Starting structuring model");
            foreach(ICountry country in Database.Countries.Values)
            {
                foreach(IRegion region in country.Regions.Values)
                {
                    Database.Regions.Add(region);

                    foreach(ICity city in region.Cities.Values)
                    {
                        Database.Cities.Add(city);
                    }
                }
            }
            Console.WriteLine("Done structuring model");

            return Database;
        }

        protected void AddLineToDatabase(string[] line)
        {
            ICountry country = new Country
            {
                Name = line[FieldCountryName]
            };

            country = Database.Countries.GetOrAdd(line[FieldCountryName], country);

            IRegion region = new Region
            {
                Name = line[FieldRegionName],
                Country = country
            };

            region = country.Regions.GetOrAdd(line[FieldRegionName], region);

            ICity city = new City
            {
                Name = line[FieldCityName],
                Country = country,
                Region = region
            };

            try
            {
                city.Population = line[FieldCityPopulation].Length > 0 ? int.Parse(line[FieldCityPopulation]) : 0;
            }
            catch (FormatException){}

            try
            {
                city.Latitude = Double.Parse(line[FieldCityLatitude]);
                city.Longitude = Double.Parse(line[FieldCityLongitude]);
            }
            catch (FormatException)
            {
                return;
            }
            
            region.Cities.GetOrAdd(line[FieldCityName], city);
        }
    }
}