using Pokemon_Review_API.Models;

namespace Pokemon_Review_API.InterFace
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetAll();
        Task<Owner> GetById(int id);
        ICollection<Owner> GetOwnerOfAPokemon(int pokeId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        bool OwnerExists(int OWnerId);
        Task<Owner> Add(Owner owner);
        Owner updata(Owner owner);
        Owner Deleat(Owner owner);
    }
}
