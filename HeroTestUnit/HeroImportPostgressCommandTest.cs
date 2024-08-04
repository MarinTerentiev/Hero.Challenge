using Application.Common.Interfaces;
using Application.HeroImportPostgressComponent.Commands;
using Domain.Entities;
using Moq;

namespace HeroTestUnit;

public class HeroImportPostgressCommandTest
{
    private readonly Mock<IPostgressHeroImportRepository> _repositoryMock;
    private readonly BulkInsertHeroImportCommandHandler _handler;

    public HeroImportPostgressCommandTest()
    {
        _repositoryMock = new Mock<IPostgressHeroImportRepository>();
        _handler = new BulkInsertHeroImportCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_CallsBulkInsertHeroesAsync_WithCorrectHeroes()
    {
        // Arrange
        var heroes = new List<HeroImport>
        {
            new HeroImport { Id = 1, Name = "Hero1", Class = "Warrior", Story = "Story1", Weapon = Domain.Enums.Weapon.Sword },
            new HeroImport { Id = 2, Name = "Hero2", Class = "Mage", Story = "Story2", Weapon = Domain.Enums.Weapon.Spear }
        };

        var command = new BulkInsertHeroImportCommand { Heroes = heroes };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(repo => repo.BulkInsertHeroesAsync(heroes), Times.Once);
    }
}
