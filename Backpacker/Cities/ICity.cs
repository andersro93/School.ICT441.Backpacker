using Backpacker.Countries;
using Backpacker.Regions;

namespace Backpacker.Cities
{
    public interface ICity
    {
        string Name { get; set; }
        
        int Population { get; set; }
        
        double Latitude { get; set; }
        
        double Longitude { get; set; }
        
        ICountry Country { get; set; }
        
        IRegion Region { get; set; }
    }
}