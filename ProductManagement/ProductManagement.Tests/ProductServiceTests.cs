using AutoMapper;
using Moq;
using ProductManagement.Application.DTOs;
using ProductManagement.Application.ProductService;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductManagement.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly IMapper _mapper;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>().ReverseMap();
            });

            _mapper = config.CreateMapper();

            _productService = new ProductService(_productRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPRoducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1",Description = "description",  Price = 10, Stock = 1 },
                new Product { Id = 2, Name = "Product 2",Description = "description",  Price = 10, Stock = 1 }
            };
            _productRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equals(2, result.Count());
            Assert.Equals("Product 1", result.FirstOrDefault().Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Description = "description", Price = 10, Stock = 1 };
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equals("Product 1", result.Name);
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepositoryAddAsync()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Product 1", Description = "description", Price = 10, Stock = 1 };

            // Act
            await _productService.AddAsync(productDto);

            // Assert
            _productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryUpdateAsync()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Product 1", Description = "description", Price = 10, Stock = 1 };
            // Act
            await _productService.UpdateAsync(productDto);

            // Assert
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDeleteAsync()
        {
            // Arrange
            var productId = 1;

            // Act
            await _productService.DeleteAsync(productId);

            // Assert
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        }
    }
}
