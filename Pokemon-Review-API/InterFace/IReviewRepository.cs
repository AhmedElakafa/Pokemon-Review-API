using Pokemon_Review_API.Models;

namespace Pokemon_Review_API.InterFace
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAll();
        Task<Review> GetById(int id);
        ICollection<Review> GetReviewsOfAPokemon(int pokeId);
        bool ReviewerExists(int ReviewId);
        Task<Review> Add(Review review);
        Review Updata(Review review);
        Review Deleat(Review review);
    }
}
