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
            
            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    Lines.Add(reader.ReadLine().Split(CsvDelimeter));
                }
            }
            
            Database = new Database.Database();

            Parallel.ForEach(Lines, _parallelOptions, line => AddLineToDatabase(line));

            Parallel.ForEach(Database.Countries.Values, _parallelOptions, country =>
            {
                Parallel.ForEach(country.Regions.Values, _parallelOptions, region =>
                {
                    Database.Regions.Add(region);

                    Parallel.ForEach(region.Cities.Values, _parallelOptions, city =>
                    {
                        Database.Cities.Add(city);
                    });
                });
            });

            return Database;
        }

        protected void AddLineToDatabase(string[] line)
        {
            ICountry country = new Country();
            country.Name = line[FieldCountryName];

            country = Database.Countries.GetOrAdd(line[FieldCountryName], country);

            IRegion region = new Region();
            region.Name = line[FieldRegionName];
            region.Country = country;

            region = country.Regions.GetOrAdd(line[FieldRegionName], region);

            ICity city = new City();
            city.Name = line[FieldCityName];

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
            
            city.Region = region;
            city.Country = country;

            region.Cities.GetOrAdd(line[FieldCityName], city);
        }
    }
}