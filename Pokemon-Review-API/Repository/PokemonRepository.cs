using Microsoft.EntityFrameworkCore;
using Pokemon_Review_API.Data;
using Pokemon_Review_API.InterFace;
using Pokemon_Review_API.Models;

namespace Pokemon_Review_API.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly ApplictionDBCotext _dBCotext;

        public PokemonRepository(ApplictionDBCotext dBCotext )
        {
            _dBCotext = dBCotext;
        }
        public Pokemon CreatePokemon(int OwnerID, int CategoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _dBCotext.Owners.Where(a => a.Id == OwnerID).FirstOrDefault();
            var category = _dBCotext.Categories.Where(a => a.Id == CategoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon,
            };

            _dBCotext.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };

            _dBCotext.Add(pokemonCategory);
            _dBCotext.Pokemon.AddAsync(pokemon);
            _dBCotext.SaveChanges();
            return pokemon;
        }
    

        public  IEnumerable<Pokemon> GetAll()
        {
            return _dBCotext.Pokemon.OrderBy(p => p.Name).ToList();
        }

        public  Pokemon GetById(int id)
        {
            return  _dBCotext.Pokemon.SingleOrDefault(p => p.Id == id);
        }

        public async Task<Pokemon> GetByNam(string nam)
        {
            return await _dBCotext.Pokemon.SingleOrDefaultAsync(pn => pn.Name == nam);

        }

        public decimal GetRating(int pockID)
        {
            var reviews=_dBCotext.Reviews.Where(p=>p.Pokemon.Id == pockID);
            if(reviews.Count()<=0)
                return 0;
            return ((decimal)reviews.Sum(r=>r.Rating)) / reviews.Count();
        }

        public async Task<bool> isvaild(int id)
        {
            return await _dBCotext.Pokemon.AnyAsync(x => x.Id == id);
        }

       public Pokemon UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _dBCotext.Update(pokemon);
            _dBCotext.SaveChanges();
            return pokemon;
        }

        public Pokemon Deleat(Pokemon pokemon)
        {
            _dBCotext.Remove(pokemon);
            _dBCotext.SaveChanges();
            return pokemon;
        }

        
    }
}
