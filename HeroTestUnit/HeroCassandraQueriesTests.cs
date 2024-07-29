using Application.Common.Interfaces;
using Application.HeroCassandraComponent.Queries;
using Domain.Entities;
using Domain.Enums;
using Moq;

namespace HeroTestUnit;

public class HeroCassandraQueriesTests
{
    [Fact]
    public async Task Handle_GivenValidRequest_ShouldReturnListOfHeroes()
    {
        // Arrange
        var heroes = new List<Hero>
        {
            new Hero { Id = Guid.NewGuid(), Name = "Hero 1", Class = "Warrior", Story = "Story 1", Weapon = Weapon.Sword },
            new Hero { Id = Guid.NewGuid(), Name = "Hero 2", Class = "Mage", Story = "Story 2", Weapon = Weapon.Bow }
        };
        var query = new GetAllHeroQuery();

        var repositoryMock = new Mock<ICassandraNeroRepository>();
        repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(heroes);
        var handler = new GetAllHeroQueryHandler(repositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(heroes, result);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ShouldReturnHero()
    {
        // Arrange
        var heroId = Guid.NewGuid();
        var hero = new Hero { Id = heroId, Name = "Hero 1", Class = "Warrior", Story = "Story 1", Weapon = Weapon.Sword };
        var query = new GetByIdHeroQuery { Id = heroId };

        var repositoryMock = new Mock<ICassandraNeroRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(heroId)).ReturnsAsync(hero);
        var handler = new GetByIdHeroQueryHandler(repositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(hero, result);
    }
}
