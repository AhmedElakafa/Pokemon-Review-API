using Microsoft.EntityFrameworkCore;
using Pokemon_Review_API.Data;
using Pokemon_Review_API.InterFace;
using Pokemon_Review_API.Models;

namespace Pokemon_Review_API.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly ApplictionDBCotext _dBCotext;

        public ReviewerRepository(ApplictionDBCotext dBCotext)
        {
             _dBCotext = dBCotext;
        }
        public bool ReviewerExists(int ReviewerID)
        {
            return _dBCotext.Reviewers.Any(p => p.Id == ReviewerID);
        }
        public async Task<Reviewer> Add(Reviewer reviewer)
        {
            await _dBCotext.Reviewers.AddAsync(reviewer);
            _dBCotext.SaveChanges();
            return reviewer;
        }

        public async Task<IEnumerable<Reviewer>> GetAll()
        {
            var Revwers = await _dBCotext.Reviewers.ToListAsync();
            return Revwers;
        }

        public async Task<Reviewer> GetById(int id)
        {
            var Revwers = await _dBCotext.Reviewers.SingleOrDefaultAsync(c => c.Id == id);
            return Revwers;
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _dBCotext.Reviewers.Where(c => c.Id == reviewerId).Select(e => e.Reviews).FirstOrDefault();
        }

        public Reviewer Updata(Reviewer reviewer)
        {
            _dBCotext.Update(reviewer);
            _dBCotext.SaveChanges();
            return reviewer; 
        }

        public Reviewer Deleat(Reviewer reviewer)
        {
            _dBCotext.Remove(reviewer);
            _dBCotext.SaveChanges();
            return reviewer;
        }
    }
}
