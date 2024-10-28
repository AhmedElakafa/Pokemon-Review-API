using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pokemon_Review_API.Data;
using Pokemon_Review_API.Models;
using Pokemon_Review_API.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PokemonReviewApi.Repository;

public class CountryRepositoryTests
{
    private readonly CountryRepository _repository;
    private readonly ApplictionDBCotext _context;

    public CountryRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplictionDBCotext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplictionDBCotext(options);
        _repository = new CountryRepository(_context);

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        // Seed initial data for testing
        _context.Countries.AddRange(
            new Country { Id = 1, Name = "Country1" },
            new Country { Id = 2, Name = "Country2" }
        );
      
        _context.SaveChanges();
    }

    [Fact]
    public async Task Add_AddsNewCountry_ReturnsCountry()
    {
        // Arrange
        var country = new Country { Id = 3, Name = "Country3" };

        // Act
        var result = await _repository.Add(country);

        // Assert
        Assert.Equal(country, result);
        Assert.Equal(3, _context.Countries.Count());
    }

    [Fact]
    public void CountryExists_ReturnsTrue_IfCountryExists()
    {
        // Act
        var exists = _repository.CountryExists(1);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task GetAll_ReturnsAllCountries()
    {
        // Act
        var result = await _repository.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetById_ReturnsCorrectCountry()
    {
        // Act
        var result = await _repository.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Country1", result.Name);
    }

    [Fact]
    public async Task Update_UpdatesExistingCountry_ReturnsCountry()
    {
        // Arrange
        var country = await _repository.GetById(1);
        country.Name = "UpdatedCountry";

        // Act
        var result = _repository.updata(country);

        // Assert
        Assert.Equal("UpdatedCountry", result.Name);

        // Fetch the country again to ensure the update was successful
        var updatedCountry = await _repository.GetById(1);
        Assert.Equal("UpdatedCountry", updatedCountry.Name);
    }

    [Fact]
    public async Task Delete_RemovesCountry_ReturnsCountry()
    {
        // Arrange
        var country = await _repository.GetById(2);

        // Act
        var result = _repository.Deleat(country);

        // Assert
        Assert.Equal(country, result);
        Assert.Null(await _repository.GetById(2));
    }
}
