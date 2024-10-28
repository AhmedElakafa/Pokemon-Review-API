using Pokemon_Review_API.Models;

namespace Pokemon_Review_API.InterFace
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAll();
        Task<Country> GetById(int id);
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner> GetOwnersFromACountry(int countryId);
        bool CountryExists(int  CountryId);
        Task<Country> Add(Country country);
        Country updata(Country country);
        Country Deleat(Country country);
    }
}
