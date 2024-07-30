using Application.HeroCassandraComponent.Commands;
using Application.HeroCassandraComponent.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;

namespace HeroTestUnit;

public class HeroCassandraControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly HeroCassandraController _controller;

    public HeroCassandraControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new HeroCassandraController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAllHeroes_ReturnsOkResult_WithListOfHeroes()
    {
        // Arrange
        var heroes = new List<Hero> { new Hero { Id = Guid.NewGuid(), Name = "Hero1" } };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllHeroQuery>(), default))
                     .ReturnsAsync(heroes);

        // Act
        var result = await _controller.GetAllHeroes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnHeroes = Assert.IsType<List<Hero>>(okResult.Value);
        Assert.Single(returnHeroes);
    }

    [Fact]
    public async Task Get_ReturnsOkResult_WithHero()
    {
        // Arrange
        var heroId = Guid.NewGuid();
        var hero = new Hero { Id = heroId, Name = "Hero1" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetByIdHeroQuery>(), default))
                     .ReturnsAsync(hero);

        // Act
        var result = await _controller.Get(heroId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnHero = Assert.IsType<Hero>(okResult.Value);
        Assert.Equal(heroId, returnHero.Id);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenHeroNotFound()
    {
        // Arrange
        var heroId = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetByIdHeroQuery>(), default))
                     .ReturnsAsync((Hero?)null);

        // Act
        var result = await _controller.Get(heroId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Post_ReturnsCreatedAtActionResult_WithHero()
    {
        // Arrange
        var hero = new Hero { Id = Guid.NewGuid(), Name = "Hero1" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<AddHeroCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(hero.Id);

        // Act
        var result = await _controller.Post(hero);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = createdAtActionResult.Value;
        var returnHeroIdProperty = returnValue?.GetType().GetProperty("id");
        Assert.NotNull(returnHeroIdProperty);

        var returnHeroId = (Guid?)returnHeroIdProperty?.GetValue(returnValue);
        Assert.Equal(hero.Id, returnHeroId);
        Assert.Equal(nameof(HeroCassandraController.Get), createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task UpdateHero_ReturnsNoContent()
    {
        // Arrange
        var heroId = Guid.NewGuid();
        var hero = new Hero { Id = heroId, Name = "Hero1" };

        // Act
        var result = await _controller.UpdateHero(heroId, hero);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateHero_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var heroId = Guid.NewGuid();
        var hero = new Hero { Id = Guid.NewGuid(), Name = "Hero1" };

        // Act
        var result = await _controller.UpdateHero(heroId, hero);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Hero ID mismatch", badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteHero_ReturnsNoContent()
    {
        // Arrange
        var heroId = Guid.NewGuid();

        // Act
        var result = await _controller.DeleteHero(heroId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

}
