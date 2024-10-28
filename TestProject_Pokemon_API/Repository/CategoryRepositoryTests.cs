using Microsoft.EntityFrameworkCore;
using Pokemon_Review_API.Data;
using Pokemon_Review_API.Models;
using Pokemon_Review_API.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PokemonReviewApi.Repository

{
    public class CategoryRepositoryTests
    {
        private readonly ApplictionDBCotext _context;
        private readonly CategoryRepository _repository;

        public CategoryRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplictionDBCotext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplictionDBCotext(options);
            _repository = new CategoryRepository(_context);

            // Populate the in-memory database with initial data
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category1" },
                new Category { Id = 2, Name = "Category2" }
            };

            _context.Categories.AddRange(categories);
            _context.SaveChanges();
        }

        [Fact]
        public async Task Create_AddsNewCategory_ReturnsCategory()
        {
            // Arrange
            var category = new Category { Id = 3, Name = "Category3" };

            // Clear any tracked entities to prevent tracking conflicts
            _context.ChangeTracker.Clear();

            // Act
            var result = await _repository.Create(category);

            // Assert
            Assert.NotNull(result); // Ensure result is not null
            Assert.Equal(category.Id, result.Id); // Compare by Id to ensure correct insertion
            Assert.Equal(category.Name, result.Name); // Compare by Name to ensure correct data
            Assert.Equal(3, _context.Categories.Count()); // Ensure the total count matches expected
        }


        [Fact]
        public void CategoryExists_ReturnsTrue_IfCategoryExists()
        {
            // Clear any tracked entities to prevent tracking conflicts
            _context.ChangeTracker.Clear();

            // Arrange
            var category = new Category { Id = 1, Name = "UpdatedName" };
            _context.Categories.Attach(category);
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();

            // Detach the entity after saving to avoid tracking conflicts
            _context.Entry(category).State = EntityState.Detached;

            // Act
            var exists = _repository.CategoryExists(1);

            // Assert
            Assert.True(exists);
        }


        [Fact]
        public async Task GetAllCategory_ReturnsAllCategories()
        {
            // Arrange
            // Clear any tracked entities to prevent tracking conflicts
            _context.ChangeTracker.Clear();

            // Optionally, seed the database with known data if needed
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Id = 1, Name = "Category1" });
                _context.Categories.Add(new Category { Id = 2, Name = "Category2" });
                await _context.SaveChangesAsync();
            }

            // Act
            var result = await _repository.GetAllCategory();

            // Assert
            var categoriesList = result.ToList(); // Convert to List for easy counting
            Assert.NotNull(result); // Ensure result is not null
            Assert.Equal(2, categoriesList.Count); // Ensure the correct number of categories
        }


        [Fact]
        public async Task GetById_ReturnsCorrectCategory()
        {
            // Arrange
            // Clear any tracked entities to prevent tracking conflicts
            _context.ChangeTracker.Clear();

            // Seed the database with known data if needed
            if (!_context.Categories.Any(c => c.Id == 1))
            {
                _context.Categories.Add(new Category { Id = 1, Name = "Category1" });
                await _context.SaveChangesAsync();
            }

            // Act
            var result = await _repository.GetById(1);

            // Assert
            Assert.NotNull(result); // Ensure that the result is not null
            Assert.Equal(1, result.Id); // Ensure that the ID is correct
            Assert.Equal("Category1", result.Name); // Ensure that the name is correct
        }


        [Fact]
        public async Task GetPokemonsByCategory_ReturnsPokemonsForCategory()
        {
            // Arrange
            var categoryId = 1;
            var pokemons = new List<Pokemon>
            {
                new Pokemon { Id = 1, Name = "Pokemon1" },
                new Pokemon { Id = 2, Name = "Pokemon2" }
            };

            // Clear existing data and add known data
            _context.ChangeTracker.Clear();
            _context.Pokemon.AddRange(pokemons);
            _context.PokemonCategories.AddRange(pokemons.Select(p => new PokemonCategory { PokemonId = p.Id, CategoryId = categoryId }));
            await _context.SaveChangesAsync();


            // Act
            var result = await _repository.GetPokemonsByCategory(categoryId);

            // Assert
            Assert.NotNull(result); // Ensure that result is not null
            Assert.Equal(2, result.Count); // Ensure that the number of pokemons is correct
            Assert.Contains(result, p => p.Name == "Pokemon1"); // Ensure that Pokemon1 is included
            Assert.Contains(result, p => p.Name == "Pokemon2"); // Ensure that Pokemon2 is included
        }

        [Fact]
        public void Update_UpdatesExistingCategory_ReturnsCategory()
        {
            // Arrange
            // Clear any tracked entities to prevent tracking conflicts
            _context.ChangeTracker.Clear();

            // Retrieve an existing category from the database
            var category = _context.Categories.First();
            category.Name = "UpdatedCategory";

            // Act
            var result = _repository.updata(category);

            // Assert
            Assert.NotNull(result); // Ensure that result is not null
            Assert.Equal("UpdatedCategory", result.Name); // Ensure that the name is updated correctly

            // Fetch the category again from the database to confirm the update
            var updatedCategory = _context.Categories.Find(category.Id);
            Assert.NotNull(updatedCategory); // Ensure that the updated category still exists in the database
            Assert.Equal("UpdatedCategory", updatedCategory.Name); // Confirm that the name was updated
        }


        [Fact]
        public void Delete_RemovesCategory_ReturnsCategory()
        {
            // Arrange
            var category = _context.Categories.First();

            // Act
            var result = _repository.Deleat(category);

            // Assert
            Assert.Equal(category, result);
            Assert.Equal(1, _context.Categories.Count());
        }
    }
}
