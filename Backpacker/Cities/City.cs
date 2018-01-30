using Backpacker.Countries;
using Backpacker.Regions;

namespace Backpacker.Cities
{
    public class City : ICity
    {
        public string Name { get; set; }
        
        public int Population { get; set; }
        
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }

        public ICountry Country { get; set; }
        
        public IRegion Region { get; set; }
    }
}