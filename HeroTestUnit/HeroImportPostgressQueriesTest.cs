using Application.Common.Interfaces;
using Application.HeroImportPostgressComponent.Queries;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroTestUnit;

public class HeroImportPostgressQueriesTest
{
    private readonly Mock<IPostgressHeroImportRepository> _repositoryMock;
    private readonly GetBySeedIdHeroImportQueryHandler _handler;

    public HeroImportPostgressQueriesTest()
    {
        _repositoryMock = new Mock<IPostgressHeroImportRepository>();
        _handler = new GetBySeedIdHeroImportQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsHeroes_WhenHeroesExist()
    {
        // Arrange
        var seedId = Guid.NewGuid();
        var expectedHeroes = new List<HeroImport>
        {
            new HeroImport { Id = 1, Name = "Hero1", Class = "Warrior", Story = "Story1", Weapon = Domain.Enums.Weapon.Bow,  SeedId = seedId },
            new HeroImport { Id = 2, Name = "Hero2", Class = "Mage", Story = "Story2", Weapon = Domain.Enums.Weapon.Sword, SeedId = seedId }
        };

        _repositoryMock.Setup(repo => repo.GetHeroesBySeedIdAsync(seedId))
            .ReturnsAsync(expectedHeroes);

        var query = new GetBySeedIdHeroImportQuery { SeedId = seedId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedHeroes, result);
        _repositoryMock.Verify(repo => repo.GetHeroesBySeedIdAsync(seedId), Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoHeroesExist()
    {
        // Arrange
        var seedId = Guid.NewGuid();
        var expectedHeroes = new List<HeroImport>();

        _repositoryMock.Setup(repo => repo.GetHeroesBySeedIdAsync(seedId))
            .ReturnsAsync(expectedHeroes);

        var query = new GetBySeedIdHeroImportQuery { SeedId = seedId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _repositoryMock.Verify(repo => repo.GetHeroesBySeedIdAsync(seedId), Times.Once);
    }
}
