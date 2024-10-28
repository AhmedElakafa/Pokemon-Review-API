using Microsoft.EntityFrameworkCore;
using Pokemon_Review_API.Data;
using Pokemon_Review_API.InterFace;
using Pokemon_Review_API.Models;
using System.Diagnostics.Metrics;

namespace Pokemon_Review_API.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly ApplictionDBCotext _dBCotext;

        public OwnerRepository(ApplictionDBCotext dBCotext)
        {
            _dBCotext = dBCotext;
        }
        public bool OwnerExists(int OWnerId)
        {
            return _dBCotext.Owners.Any(p => p.Id == OWnerId);
        }
        public async Task<Owner> Add(Owner owner)
        {
            await _dBCotext.Owners.AddAsync(owner);
            _dBCotext.SaveChanges();
            return owner;
        }

        public async Task<IEnumerable<Owner>> GetAll()
        {
            var Owners = await _dBCotext.Owners.OrderBy(c => c.Id).ToListAsync();
            return Owners;
        }

        public async Task<Owner> GetById(int id)
        {
            var Owners = await _dBCotext.Owners.SingleOrDefaultAsync(c => c.Id == id);
            return Owners;
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
            return _dBCotext.PokemonOwners.Where(c => c.PokemonId == pokeId).Select(e => e.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _dBCotext.PokemonOwners.Where(c => c.Owner.Id == ownerId).Select(e => e.Pokemon).ToList();
        }
        public Owner updata(Owner owner)
        {
            _dBCotext.Update(owner);
            _dBCotext.SaveChanges();
            return owner;
        }
        public Owner Deleat(Owner owner)
        {
            _dBCotext.Remove(owner);
            _dBCotext.SaveChanges();
            return owner;
        }
    }
}
