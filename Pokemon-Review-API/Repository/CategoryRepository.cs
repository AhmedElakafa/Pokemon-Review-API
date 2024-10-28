using Microsoft.EntityFrameworkCore;
using Pokemon_Review_API.Data;
using Pokemon_Review_API.InterFace;
using Pokemon_Review_API.Models;

namespace Pokemon_Review_API.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplictionDBCotext _dBCotext;

        public CategoryRepository(ApplictionDBCotext dBCotext)
        {
            _dBCotext = dBCotext;
        }
        public async Task< Category> Create(Category category)
        {
            await _dBCotext.AddAsync(category);
            _dBCotext.SaveChanges();
             return  category;
        }

        public bool CategoryExists(int pokeId)
        {

            return _dBCotext.Categories
                     .AsNoTracking()
                     .Any(p => p.Id == pokeId);
        }
       
        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            var Cate = await _dBCotext.Categories.OrderBy(c => c.Id).ToListAsync();
            return Cate;
        }

        public async Task<Category> GetById(int id)
        {
            var Cate = await _dBCotext.Categories.SingleOrDefaultAsync(c => c.Id==id);
            return Cate;
        }

        public async Task<ICollection<Pokemon>> GetPokemonsByCategory(int categoryId)
        {
            var Cate = await _dBCotext.PokemonCategories.Where(c => c.CategoryId == categoryId).Select(e=>e.Pokemon).ToListAsync();
            return Cate; 
        }


        public Category updata(Category category)
        {
            _dBCotext.Update(category);
            _dBCotext.SaveChanges();
            return category;
        }
        public Category Deleat(Category category)
        {
            _dBCotext.Remove(category);
            _dBCotext.SaveChanges();
            return category;
        }
    }
}
