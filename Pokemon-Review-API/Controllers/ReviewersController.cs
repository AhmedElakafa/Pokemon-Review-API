using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokemon_Review_API.DTO;
using Pokemon_Review_API.InterFace;
using Pokemon_Review_API.Models;
using Pokemon_Review_API.Repository;

namespace Pokemon_Review_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewersController : ControllerBase
    {
        private readonly IReviewerRepository _reviewer;
        private readonly IMapper _mapper;

        public ReviewersController(IReviewerRepository reviewer ,IMapper mapper)
        {
            _reviewer = reviewer;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllAsunc()
        {
            var reviewer = await _reviewer.GetAll();
            var reviewerDto = _mapper.Map<List<ReviewerDto>>(reviewer);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewerDto);
        }
        [HttpGet("{reviewerById}")]
        public async Task<IActionResult> GetreviewerById(int reviewerById)
        {
            if (!_reviewer.ReviewerExists(reviewerById))
                return NotFound();
            var reviewer = await _reviewer.GetById(reviewerById);
            var reviewerDto = _mapper.Map<ReviewerDto>(reviewer);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewerDto);
        }
        [HttpGet("Review/ReviwerById")]
        public IActionResult GetReviewsByReviewerId(int ReviwerById)
        {
            if (!_reviewer.ReviewerExists(ReviwerById))
                return NotFound();
            var RevieDtos = _mapper.Map<List<ReviwDto>>(_reviewer.GetReviewsByReviewer(ReviwerById));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(RevieDtos);

        }
        [HttpPost]
        public async Task<IActionResult> CreateReviewer([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);

            var Reviewers = await _reviewer.GetAll();
            var Reviewer = Reviewers
                .Where(c => c.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (Reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

            if (_reviewer.Add(reviewerMap)==null)
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
        [HttpPut("{reviewerId}")]
        public IActionResult updatedReview(int reviewerId, [FromBody] ReviewerDto updatedReviewer)
        {
            if (updatedReviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != updatedReviewer.Id)
                return BadRequest(ModelState);

            if (!_reviewer.ReviewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewerMap = _mapper.Map<Reviewer>(updatedReviewer);

            if ( _reviewer.Updata(reviewerMap)==null)
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return Ok(reviewerMap);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reviewer =await _reviewer.GetById(id);

            if (reviewer == null)
                return NotFound($"Not Found Data Withe Id {id} ");

            _reviewer.Deleat(reviewer);
            return Ok(reviewer);
        }
    }
}
