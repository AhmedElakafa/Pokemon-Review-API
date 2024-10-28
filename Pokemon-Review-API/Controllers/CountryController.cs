using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Review_API.DTO;
using Pokemon_Review_API.InterFace;
using Pokemon_Review_API.Models;
using System.Diagnostics.Metrics;

namespace Pokemon_Review_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository , IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllAsunc()
        {
            var Cuntry = await _countryRepository.GetAll();
            var CuntryDto = _mapper.Map<List<CountryDto>>(Cuntry);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(CuntryDto);
        }
        [HttpGet("{CountryById}")]
        public async Task<IActionResult> GetCountryById(int CountryById)
        {
            if (!_countryRepository.CountryExists(CountryById))
                return NotFound();
            var Counrty = await _countryRepository.GetById(CountryById);
            var countryDto = _mapper.Map<CountryDto>(Counrty);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countryDto);
        }
        [HttpGet("Owner/CountryById")]
        public IActionResult GetCountryByOwnerID(int OwunerId)
        {
            if (!_countryRepository.CountryExists(OwunerId))
                return NotFound();
            var ContryDtos = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(OwunerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(ContryDtos);

        }
        [HttpGet("CoyntryID")]
        public IActionResult GetOwnersFromACountryID(int CoyntryID)
        {
            if (!_countryRepository.CountryExists(CoyntryID))
                return NotFound();
            var OwnerDtos = _mapper.Map<List<OwnerDto>>(_countryRepository.GetOwnersFromACountry(CoyntryID));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(OwnerDtos);

        }
        [HttpPost]
        public async Task<IActionResult> CreateCountryAsync(CountryDto countryDto)
        {
            if (countryDto == null)
                return BadRequest(ModelState);

            var Counts = await _countryRepository.GetAll();
            var Count = Counts
                .Where(c => c.Name.Trim().ToUpper() == countryDto.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (Count != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countrymap = _mapper.Map<Country>(countryDto);

            if (_countryRepository.Add(countrymap) == null)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(countrymap);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CountryDto countryDto)
        {
            var country = await _countryRepository.GetById(id);

            if (country == null)
                return NotFound($"Not Found Data With Id {id}");

            country.Name = countryDto.Name;

            if (_countryRepository.updata(country)==null)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(country);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsunk(int id)
        {
            var Counts = await _countryRepository.GetById(id);

            if (Counts == null)
                return NotFound($"Not Found Data Withe Id {id} ");
            _countryRepository.Deleat(Counts);
            return Ok(Counts);
        }
    }
}
