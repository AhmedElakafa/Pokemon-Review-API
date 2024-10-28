using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Review_API.DTO;
using Pokemon_Review_API.InterFace;
using Pokemon_Review_API.Models;
using Pokemon_Review_API.Repository;

namespace Pokemon_Review_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _review;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewsController(IReviewRepository review,
            IMapper mapper, IPokemonRepository pokemonRepository
            , IReviewerRepository reviewerRepository)
        {
            _review = review;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllAsunc()
        {
            var review = await _review.GetAll();
            var reviewDto = _mapper.Map<List<ReviwDto>>(review);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewDto);
        }

        [HttpGet("{reviewById}")]
        public async Task<IActionResult> GetRevieById(int reviewById)
        {
            if (!_review.ReviewerExists(reviewById))
                return NotFound();
            var review = await _review.GetById(reviewById);
            var reviewDto = _mapper.Map<ReviwDto>(review);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewDto);
        }
        [HttpGet("Review/PoKemonById")]
        public IActionResult GetReviewsOfAPokemonID(int PoKemonById)
        {

            if (!_review.ReviewerExists(PoKemonById))
                return NotFound();
            var RevieDtos = _mapper.Map<List<ReviwDto>>(_review.GetReviewsOfAPokemon(PoKemonById));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(RevieDtos);

        }
        [HttpPost]
        public async Task<IActionResult> CreateCReviewAsync([FromQuery] int reviewerId, [FromQuery] int pokeId, [FromBody]ReviwDto reviewDto)
        {
            if (reviewDto == null)
                return BadRequest(ModelState);

            var Reviews = await _review.GetAll();
            var Review= Reviews
                .Where(c => c.Title.Trim().ToUpper() == reviewDto.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (Review != null)
            {
                ModelState.AddModelError("", "ٌReview already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Reviewmap = _mapper.Map<Review>(reviewDto);
            Reviewmap.Pokemon =  _pokemonRepository.GetById(pokeId);
            Reviewmap.Reviewer = await _reviewerRepository.GetById(reviewerId);


            if (await _review.Add(Reviewmap) == null)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(Reviewmap);
        }
        [HttpPut("{reviewId}")]
        public IActionResult updatedReview(int reviewId, [FromBody] ReviwDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);

            if (reviewId != updatedReview.Id)
                return BadRequest(ModelState);

            if (!_review.ReviewerExists(reviewId))
                return NotFound();
          
            if (!ModelState.IsValid) 
                return BadRequest();

            var reviewMap = _mapper.Map<Review>(updatedReview);

            if (_review.Updata(reviewMap)==null)
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }

            return Ok(reviewMap);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _review.GetById(id);

            if (review == null)
                return NotFound($"Not Found Data Withe Id {id} ");

            _review.Deleat(review);
            return Ok(review);
        }
    }
}

