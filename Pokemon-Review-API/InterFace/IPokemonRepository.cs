using Pokemon_Review_API.Models;

namespace Pokemon_Review_API.InterFace
{
    public interface IPokemonRepository
    {
        IEnumerable<Pokemon> GetAll();
        Pokemon GetById(int id);
        Task<Pokemon> GetByNam(string nam);
        decimal GetRating(int pokid);
        Pokemon CreatePokemon(int OwnerID,int CategoryId, Pokemon Pockemon);
        Pokemon UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        Pokemon Deleat(Pokemon pokemon);
        Task<bool> isvaild(int id);

    }
}
