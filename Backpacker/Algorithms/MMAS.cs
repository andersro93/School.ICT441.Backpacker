using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Backpacker.Ants;
using Backpacker.Cities;
using Backpacker.Countries;
using Backpacker.Database;
using Backpacker.Exceptions;
using Backpacker.Performances;
using Backpacker.Regions;
using Backpacker.Roulettes;

namespace Backpacker.Algorithms
{
    public class MMAS : IAlgorithm
    {
        public IDatabase Database { get; set; }
        
        public IAnt BestAnt { get; set; }
        
        protected double BestAntPerformance;
        
        protected int TraverseLevel = 3;
        
        protected int UnchangedBestCycleCounter = 0;
        
        protected const int ConvergeAmount = 2000;
        
        protected ConcurrentDictionary<int, ICountry> countrySequence = new ConcurrentDictionary<int, ICountry>();
        
        protected ConcurrentDictionary<int, IRegion> regionSequence = new ConcurrentDictionary<int, IRegion>();
        
        protected ConcurrentDictionary<ICity, IRoulette<ICity>> cityRoulettes = new ConcurrentDictionary<ICity, IRoulette<ICity>>();
        
        protected ConcurrentDictionary<IRegion, IRoulette<IRegion>> regionRoulettes = new ConcurrentDictionary<IRegion, IRoulette<IRegion>>();
        
        protected ConcurrentDictionary<ICountry, IRoulette<ICountry>> countryRoulettes = new ConcurrentDictionary<ICountry, IRoulette<ICountry>>();


        public void MoveToNextCity(IAnt ant)
        {
            ICity city = Roulette(ant);
            
            ant.Visited.Add(city);
            ant.Current = city;
        }

        public void AntDoneWalking(IAnt ant)
        {
            double antPerformance = AntPerformanceCalculator.CalculateAntPerformance(ant);

            if (antPerformance <= BestAntPerformance)
            {
                UnchangedBestCycleCounter = 0;
                BestAnt = ant;
                return;
            }
            
            UnchangedBestCycleCounter += 1;

            DetermineTraverseLevel();
        }

        protected ICity Roulette(IAnt ant)
        {
            if (ant.Visited.Count-1 == ant.CitiesToVisit)
            {
                return ant.Colony.Destination;
            }
            
            if (TraverseLevel == 3)
            {
                ICountry country = GetCountryRouletteForAnt(ant).GetNext();
                
                IRegion region = country.Regions.FirstOrDefault().Value;

                ICity city = region.Cities.FirstOrDefault().Value;
                
                return city;
            }
            if (TraverseLevel == 2)
            {
                IRegion region = GetRegionRouletteForAnt(ant).GetNext();
                
                ICity city = region.Cities.FirstOrDefault().Value;

                return city;
            }
            
            return GetCityRouletteForAnt(ant).GetNext();
        }

        protected IRoulette<ICountry> GetCountryRouletteForAnt(IAnt ant)
        {
            ICity currentCity = ant.Current;

            ICountry currentCountry = currentCity.Country;

            IRoulette<ICountry> countryRoulette;

            if (countryRoulettes.TryGetValue(currentCountry, out countryRoulette))
            {
                return countryRoulette;
            }

            return CreateCountryRouletteForCountry(currentCountry);
        }

        protected IRoulette<IRegion> GetRegionRouletteForAnt(IAnt ant)
        {
            IRegion currentRegion = ant.Current.Region;

            IRoulette<IRegion> regionRoulette;

            if (regionRoulettes.TryGetValue(currentRegion, out regionRoulette))
            {
                return regionRoulette;
            }

            return CreateRegionRouletteForRegion(currentRegion);
        }

        protected IRoulette<ICity> GetCityRouletteForAnt(IAnt ant)
        {
            ICity currentCity = ant.Current;

            IRoulette<ICity> cityRoulette;

            if (cityRoulettes.TryGetValue(currentCity, out cityRoulette))
            {
                return cityRoulette;
            }

            return CreateCityRouletteForCity(currentCity);
        }

        protected IRoulette<ICountry> CreateCountryRouletteForCountry(ICountry country)
        {
            IRoulette<ICountry> roulette = new Roulette<ICountry>();

            List<ICountry> countries = new List<ICountry>();
            
            foreach (KeyValuePair<string,ICountry> keyValuePair in Database.Countries)
            {
                countries.Add(keyValuePair.Value);
            }

            countryRoulettes.TryAdd(country, roulette);

            return roulette.SetOptions(countries);
        }

        protected IRoulette<IRegion> CreateRegionRouletteForRegion(IRegion region)
        {
            IRoulette<IRegion> roulette = new Roulette<IRegion>();
            
            List<IRegion> regions = new List<IRegion>();
            
            foreach (KeyValuePair<int,ICountry> countryKeyValuePair in countrySequence)
            {
                foreach (KeyValuePair<string,IRegion> regionKeyValuePair in countryKeyValuePair.Value.Regions)
                {
                    if (regionKeyValuePair.Value != region && !regions.Contains(regionKeyValuePair.Value))
                    {
                        regions.Add(regionKeyValuePair.Value);
                    }
                }
            }
            
            regionRoulettes.TryAdd(region, roulette);

            return roulette.SetOptions(regions);
        }

        protected IRoulette<ICity> CreateCityRouletteForCity(ICity city)
        {
            IRoulette<ICity> roulette = new Roulette<ICity>();
            
            List<ICity> cities = new List<ICity>();
            
            foreach (KeyValuePair<int,IRegion> regionKeyValuePair in regionSequence)
            {
                foreach (KeyValuePair<string,ICity> cityKeyValuePair in regionKeyValuePair.Value.Cities)
                {
                    if (cityKeyValuePair.Value != city && !cities.Contains(cityKeyValuePair.Value))
                    {
                        cities.Add(cityKeyValuePair.Value);
                    }
                }
            }
            
            cityRoulettes.TryAdd(city, roulette);

            return roulette.SetOptions(cities);
        }

        protected void DetermineTraverseLevel()
        {
            if (UnchangedBestCycleCounter <= ConvergeAmount)
            {
                return;
            }
            
            if (TraverseLevel <= 0)
            {
                throw new ConvergedException();
            }

            TraverseLevel -= 1;
            Console.WriteLine($"Traverse level decreased to level {TraverseLevel}");

            switch (TraverseLevel)
            {
                case 2:
                    CreateCountrySequence();
                    break;
                case 1:
                    CreateRegionSequence();
                    break;
            }
        }

        protected void CreateCountrySequence()
        {
            int number = 0;
            
            foreach (ICity city in BestAnt.Visited)
            {
                countrySequence.TryAdd(number, city.Country);
            }
        }

        protected void CreateRegionSequence()
        {
            int number = 0;
            
            foreach (ICity city in BestAnt.Visited)
            {
                regionSequence.TryAdd(number, city.Region);
            }
        }
        
    }
}