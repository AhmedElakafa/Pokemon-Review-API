using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Review_API.Authorization;
using Pokemon_Review_API.DTO;
using Pokemon_Review_API.InterFace;
using Pokemon_Review_API.Models;
using Pokemon_Review_API.Repository;
using ProjectCRUD.Data;

namespace Pokemon_Review_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemon;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemon, IReviewRepository reviewRepository,
            IMapper mapper )
        {
            _pokemon = pokemon;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [CheckPermission(premission.ReadPpokemon)]
        public IActionResult GetAll()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemon.GetAll());
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            return Ok(pokemons);
        }
        [AllowAnonymous]
        [HttpGet("{pokeById}")]
        public async Task<IActionResult> GetPokemonById(int pokeById)
        {
            if (!await _pokemon.isvaild(pokeById))
             
                return NotFound();
            
            var pokemon =  _mapper.Map<PokemonDto>(_pokemon.GetById(pokeById));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }
   
        [HttpGet("{pokeRat}/rating")]
        [CheckPermission(premission.ReadPokemonReating)]
        public async Task<IActionResult> GetPokemonRating(int pokeRat)
        {
            if (! await _pokemon.isvaild(pokeRat))
                return NotFound();

            var rating = _pokemon.GetRating(pokeRat);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(rating);
        }
        [HttpPost]
        [CheckPermission(premission.AddPokemon)]
        public IActionResult CreatePokemon([FromQuery]int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemons = _pokemon.GetAll()
                .Where(p=>p.Name.Trim().ToUpper()==pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (pokemons != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);


            if (_pokemon.CreatePokemon(ownerId, categoryId, pokemonMap)==null)
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{pokeId}")]
        [CheckPermission(premission.EditPokemon)]
        public IActionResult UpdatePokemon(int pokeId,
             [FromQuery] int ownerId, [FromQuery] int catId,
             [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null || pokeId != updatedPokemon.Id)
                return BadRequest("Invalid data.");

            var existingPokemon =  _pokemon.isvaild(pokeId);
            if (existingPokemon == null)
                return NotFound("Pokemon not found.");

            // Map the updated DTO to the existing Pokemon entity
            var pokemonMap = _mapper.Map<Pokemon>(updatedPokemon);

            // Perform the update, without modifying the key (pokeId)
            if ( _pokemon.UpdatePokemon(ownerId, catId, pokemonMap) ==null)
            {
                ModelState.AddModelError("", "Something went wrong updating the Pokemon.");
                return StatusCode(500, ModelState);
            }

            return Ok(pokemonMap);
        }

        [HttpDelete("{id}")]
        [CheckPermission(premission.DeletePokemonB)]
        public IActionResult Delete(int id)
        {
            var pocke = _pokemon.GetById(id);

            if (pocke == null)
                return NotFound($"Not Found Data Withe Id {id} ");
            _pokemon.Deleat(pocke);
            return Ok(pocke);
        }
    }
}
