using Pokemon_Review_API.Models;

namespace Pokemon_Review_API.InterFace
{
    public interface ICategoryRepository
    {
       Task< IEnumerable<Category>> GetAllCategory();
        Task<Category> GetById(int id);
        Task<ICollection<Pokemon>>GetPokemonsByCategory(int categoryId);
        bool CategoryExists(int pokeId);
        Task<Category>Create(Category genre);
        Category updata(Category genre);
        Category Deleat( Category genre);
    }
}
