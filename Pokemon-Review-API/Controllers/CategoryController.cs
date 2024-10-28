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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _category;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository category, IMapper mapper)
        {
            _category = category;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [Authorize (Roles ="admin")]
        public async Task<IActionResult> GetAllAsunc()
        {
            var Catgeory = await _category.GetAllCategory();
            var categoryDtos = _mapper.Map<List<CategoryDto>>(Catgeory);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(categoryDtos);
        }
        [HttpGet("{CategoryById}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryById(int CategoryById)
        {
            if (!_category.CategoryExists(CategoryById))
                return NotFound();
            var catid = await _category.GetById(CategoryById);
            var CategoryDto = _mapper.Map<CategoryDto>(catid);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(CategoryDto);
        }
        [HttpGet("Pokemons/CategoryId")]
        [Authorize(policy: "SuperUsersonly")]
        public async Task<IActionResult> GetPokemonByCategoryID(int CategoryId)
        {
            if (!_category.CategoryExists(CategoryId))
                return NotFound();
            var catid = await _category.GetPokemonsByCategory(CategoryId);
            var pokemonDto = _mapper.Map<List<PokemonDto>>(catid);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemonDto);

        }
        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync(CategoryDto categoryDto)
        {
            if (categoryDto == null)
                return BadRequest(ModelState);

            var categories = await _category.GetAllCategory();
            var category = categories
                .Where(c => c.Name.Trim().ToUpper() == categoryDto.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryDto); 

            if (_category.Create(categoryMap) == null)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(categoryMap);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAsync(int id, CategoryDto dto)
        {
            var category = await _category.GetById(id);

            if (category == null)
                return NotFound($"Not Found Data With Id {id} ");
           // var categoryMap = _mapper.Map<Category>(dto);
            category.Name = dto.Name;

            if (_category.updata(category) ==null)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(category);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAsunk(int id)
        {
            var Category = await _category.GetById(id);

            if (Category == null)
                return NotFound($"Not Found Data Withe Id {id} ");
           _category.Deleat(Category);
            return Ok(Category);
        }
    }
}
