using System.Collections.Generic;
using System.Linq;
using Backpacker.Ants;
using Backpacker.Cities;
using Backpacker.Countries;
using Backpacker.Database;

namespace Backpacker.Algorithms
{
    public class MMAS : IAlgorithm
    {
        public IDatabase Database { get; set; }

        protected System.Random Random = new System.Random();

        protected int TraverseLevel = 3;

        protected Dictionary<int, ICountry> CountryWeights;

        public MMAS()
        {
            CountryWeights = new Dictionary<int, ICountry>();
        }

        public ICity GetNextCity(IAnt ant)
        {
            return Roulette(ant);
        }

        protected ICity Roulette(IAnt ant)
        {
            if (ant.Visited.Count-1 == ant.CitiesToVisit)
            {
                return ant.Destination;
            }
            
            if (TraverseLevel == 3)
            {
                GetNextCountry(ant);
            }
            
            return ant.Destination;
        }

        protected ICountry GetNextCountry(IAnt ant)
        {
            int sumOfCountryWieghts = CountryWeights.Sum(country => country.Key);

            int randomValue = Random.Next(0, sumOfCountryWieghts);

            ICountry selectedcountry = CountryWeights.First().Value;

            foreach (KeyValuePair<int, ICountry> keyValuePair in CountryWeights.OrderBy(pair => pair.Key))
            {
                randomValue -= keyValuePair.Key;

                if (randomValue > 0) continue;
                
                selectedcountry = keyValuePair.Value;

                break;
            }

            return selectedcountry;
        }
    }
}