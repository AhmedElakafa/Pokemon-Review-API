using Microsoft.EntityFrameworkCore;
using Pokemon_Review_API.Data;
using Pokemon_Review_API.InterFace;
using Pokemon_Review_API.Models;

namespace Pokemon_Review_API.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplictionDBCotext _dBCotext;

        public CountryRepository(ApplictionDBCotext dBCotext)
        {
            _dBCotext = dBCotext;
        }
        public async Task<Country> Add(Country country)
        {
            await _dBCotext.Countries.AddAsync(country);
            _dBCotext.SaveChanges();
            return country;
        }

        public bool CountryExists(int CountryId)
        {

            return _dBCotext.Countries.Any(p => p.Id == CountryId);
        }

       
        public async Task<IEnumerable<Country>> GetAll()
        {
            var Country = await _dBCotext.Countries.OrderBy(c => c.Id).ToListAsync();
            return Country;
        }

        public async Task<Country> GetById(int id)
        {
            var country = await _dBCotext.Countries.Where(c => c.Id == id).FirstOrDefaultAsync();
            return country;
        }

        public Country GetCountryByOwner(int ownerId)
        {
          return _dBCotext.Owners.Where(c => c.Id == ownerId).Select(e => e.Country).FirstOrDefault();
        
        }

        public ICollection<Owner> GetOwnersFromACountry(int countryId)
        {
            return _dBCotext.Owners.Where(c => c.Country.Id == countryId).ToList();

        }

        public Country updata(Country country)
        {
            _dBCotext.Update(country);
            _dBCotext.SaveChanges();
            return country;
        }
        public Country Deleat(Country country)
        {
            _dBCotext.Remove(country);
            _dBCotext.SaveChanges();
            return country;
        }
    }
}
