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
    public class OwnresController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;

        public OwnresController(IOwnerRepository ownerRepository,IMapper mapper ,
            ICountryRepository countryRepository)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllAsunc()
        {
            var Owners = await _ownerRepository.GetAll();
            var OwnersDto = _mapper.Map<List<OwnerDto>>(Owners);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(OwnersDto);
        }
        [HttpGet("{OwnerById}")]
        public async Task<IActionResult> GetOwnerById(int OwnerById)
        {
            if (!_ownerRepository.OwnerExists(OwnerById))
                return NotFound();
            var Owiners = await _ownerRepository.GetById(OwnerById);
            var OwnerDto = _mapper.Map<OwnerDto>(Owiners);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(OwnerDto);
        }
        [HttpGet("Owner/PokemonId")]
        public IActionResult GetOwnerOfAPokemonId(int POkemonID)
        {
            if (!_ownerRepository.OwnerExists(POkemonID))
                return NotFound();
            var OwnerDto = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnerOfAPokemon(POkemonID));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(OwnerDto);
        }
        [HttpGet("Pokemon/OwnerID")]
        public IActionResult GetPokemonByOwnerID(int OWnerID)
        {
            if (!_ownerRepository.OwnerExists(OWnerID))
                return NotFound();
            var pokemonDtos = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(OWnerID));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemonDtos);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOwnerAsync([FromQuery] int countryId, [FromBody] OwnerDto OwnerDto)
        {
            if (OwnerDto == null)
                return BadRequest(ModelState);

            var owners = await _ownerRepository.GetAll();
            var Owner = owners
                .Where(c => c.LastName.Trim().ToUpper() == OwnerDto.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (Owner != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(OwnerDto);

            var country = await _countryRepository.GetById(countryId);
            if (country == null)
            {
                ModelState.AddModelError("", "Country not found");
                return NotFound(ModelState);
            }

            ownerMap.Country = country;

            if ( _ownerRepository.Add(ownerMap)==null)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(ownerMap);
        }

        [HttpPut("{OwnerId}")]

        public async Task<IActionResult> UpdateAsync(int OwnerId, [FromBody] OwnerDto ownerDto)
        {
            var owner = await _ownerRepository.GetById(OwnerId);

            if (owner == null)
                return NotFound($"Not Found Data With Id {OwnerId}");

            owner.FirstName = ownerDto.FirstName;
            owner.LastName = ownerDto.LastName;
            owner.Gym = ownerDto.Gym;

            // تحديث الكائن في قاعدة البيانات
            if (_ownerRepository.updata(owner) == null)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(owner);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsunk(int id)
        {
            var owner = await _ownerRepository.GetById(id);

            if (owner == null)
                return NotFound($"Not Found Data Withe Id {id} ");
            _ownerRepository.Deleat(owner);
            return Ok(owner);
        }

    }
}
