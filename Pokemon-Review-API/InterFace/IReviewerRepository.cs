using Pokemon_Review_API.Models;

namespace Pokemon_Review_API.InterFace
{
    public interface IReviewerRepository
    {
        Task<IEnumerable<Reviewer>> GetAll();
        Task<Reviewer> GetById(int id);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        bool ReviewerExists(int ReviewerID);
        Task<Reviewer> Add(Reviewer reviewer);
        Reviewer Updata(Reviewer reviewer);
        Reviewer Deleat(Reviewer reviewer);
    }
}
