using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pokemon_Review_API.Controllers;
using Pokemon_Review_API.DTO;
using Pokemon_Review_API.InterFace;
using Pokemon_Review_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace PokemonReviewApi.TestControle
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _mockCategoryRepo = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CategoryController(_mockCategoryRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfCategoryDtos()
        {
            // Arrange
            var categories = new List<Category> { new Category { Id = 1, Name = "Electric" } };
            _mockCategoryRepo.Setup(repo => repo.GetAllCategory()).ReturnsAsync(categories);
            _mockMapper.Setup(mapper => mapper.Map<List<CategoryDto>>(It.IsAny<List<Category>>()))
                       .Returns(new List<CategoryDto> { new CategoryDto { Id = 1, Name = "Electric" } });

            // Act
            var result = await _controller.GetAllAsunc();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CategoryDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            _mockCategoryRepo.Setup(repo => repo.CategoryExists(It.IsAny<int>())).Returns(false);

            // Act
            var result = await _controller.GetCategoryById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateCategoryAsync_ReturnsOkResult_WhenCategoryIsCreated()
        {
            // Arrange
            var categoryDto = new CategoryDto { Id = 1, Name = "Electric" };
            var category = new Category { Id = 1, Name = "Electric" };
            _mockCategoryRepo.Setup(repo => repo.GetAllCategory()).ReturnsAsync(new List<Category>());
            _mockMapper.Setup(mapper => mapper.Map<Category>(It.IsAny<CategoryDto>())).Returns(category);
            _mockCategoryRepo.Setup(repo => repo.Create(It.IsAny<Category>())).ReturnsAsync(category);

            // Act
            var result = await _controller.CreateCategoryAsync(categoryDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Category>(okResult.Value);
            Assert.Equal(category.Name, returnValue.Name);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            _mockCategoryRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Category)null);

            // Act
            var result = await _controller.UpdateAsync(1, new CategoryDto { Id = 1, Name = "Electric" });

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteAsunk_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            _mockCategoryRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Category)null);

            // Act
            var result = await _controller.DeleteAsunk(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
