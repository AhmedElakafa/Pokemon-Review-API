using Microsoft.EntityFrameworkCore;
using Pokemon_Review_API.Data;
using Pokemon_Review_API.InterFace;
using Pokemon_Review_API.Models;

namespace Pokemon_Review_API.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplictionDBCotext _dBCotext;

        public ReviewRepository(ApplictionDBCotext dBCotext)
        {
            _dBCotext = dBCotext;
        }
        public bool ReviewerExists(int ReviewId)
        {
            return _dBCotext.Reviews.Any(p => p.Id == ReviewId);

        }
        public async Task<Review> Add(Review review)
        {
            await _dBCotext.Reviews.AddAsync(review);
            _dBCotext.SaveChanges();
            return review;
        }

        public async Task<IEnumerable<Review>> GetAll()
        {
            var Revws = await _dBCotext.Reviews.ToListAsync();
            return Revws;
        }

        public async Task<Review> GetById(int id)
        {
            var Revws = await _dBCotext.Reviews.SingleOrDefaultAsync(c => c.Id == id);
            return Revws;
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
        {
            return _dBCotext.Pokemon.Where(c => c.Id == pokeId).Select(e => e.Reviews).FirstOrDefault();
        }

        public Review Updata(Review review)
        {
            _dBCotext.Update(review);
            _dBCotext.SaveChanges();
            return review;
        }

        public Review Deleat(Review review)
        {
            _dBCotext.Remove(review);
            _dBCotext.SaveChanges();
            return review;
        }
    }
}
