using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pokemon_Review_API.Controllers;
using Pokemon_Review_API.DTO;
using Pokemon_Review_API.InterFace;
using Pokemon_Review_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PokemonReviewApi.TestControle
{
    public class CountryControllerTests
    {
        private readonly Mock<ICountryRepository> _mockCountryRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CountryController _controller;

        public CountryControllerTests()
        {
            _mockCountryRepo = new Mock<ICountryRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CountryController(_mockCountryRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfCountryDtos()
        {
            // Arrange
            var countries = new List<Country> { new Country { Id = 1, Name = "Egypt" } };
            _mockCountryRepo.Setup(repo => repo.GetAll()).ReturnsAsync(countries);
            _mockMapper.Setup(mapper => mapper.Map<List<CountryDto>>(It.IsAny<List<Country>>()))
                       .Returns(new List<CountryDto> { new CountryDto { Id = 1, Name = "Egypt" } });

            // Act
            var result = await _controller.GetAllAsunc();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CountryDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetCountryById_ReturnsNotFound_WhenCountryDoesNotExist()
        {
            // Arrange
            _mockCountryRepo.Setup(repo => repo.CountryExists(It.IsAny<int>())).Returns(false);

            // Act
            var result = await _controller.GetCountryById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateCountryAsync_ReturnsOkResult_WhenCountryIsCreated()
        {
            // Arrange
            var countryDto = new CountryDto { Id = 1, Name = "Egypt" };
            var country = new Country { Id = 1, Name = "Egypt" };
            _mockCountryRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Country>());
            _mockMapper.Setup(mapper => mapper.Map<Country>(It.IsAny<CountryDto>())).Returns(country);
            _mockCountryRepo.Setup(repo => repo.Add(It.IsAny<Country>())).ReturnsAsync(country);

            // Act
            var result = await _controller.CreateCountryAsync(countryDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Country>(okResult.Value);
            Assert.Equal(country.Name, returnValue.Name);
        }
        [Fact]
        public async Task UpdateAsync_ReturnsNotFound_WhenCountryDoesNotExist()
        {
            // Arrange
            _mockCountryRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Country)null);

            // Act
            var result = await _controller.UpdateAsync(1, new CountryDto { Id = 1, Name = "Egypt" });

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteAsunk_ReturnsNotFound_WhenCountryDoesNotExist()
        {
            // Arrange
            _mockCountryRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Country)null);

            // Act
            var result = await _controller.DeleteAsunk(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}