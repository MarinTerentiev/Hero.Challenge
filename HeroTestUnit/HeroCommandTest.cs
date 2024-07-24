using Application.Common.Interfaces;
using Application.HeroComponent.Commands;
using Domain.Entities;
using Domain.Enums;
using Moq;

namespace HeroTestUnit;

public class HeroCommandTest
{
    [Fact]
    public async Task Handle_GivenValidRequest_ShouldCallAddAsync()
    {
        // Arrange
        var hero = new Hero { Id = 1, Name = "Hero 1", Class = "Warrior", Story = "Story 1", Weapon = Weapon.Sword };
        var command = new AddHeroCommand { Hero = hero };

        var repositoryMock = new Mock<ICassandraNeroRepository>();
        var handler = new AddHeroCommandHandler(repositoryMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        repositoryMock.Verify(r => r.AddAsync(It.Is<Hero>(h => h.Id == hero.Id && h.Name == hero.Name)), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ShouldCallDeleteAsync()
    {
        // Arrange
        int heroId = 1;
        var command = new DeleteHeroCommand { Id = heroId };

        var repositoryMock = new Mock<ICassandraNeroRepository>();
        var handler = new DeleteHeroCommandHandler(repositoryMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        repositoryMock.Verify(r => r.DeleteAsync(heroId), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ShouldCallUpdateAsync()
    {
        // Arrange
        var hero = new Hero { Id = 1, Name = "Updated Hero", Class = "Mage", Story = "Updated Story", Weapon = Weapon.Spear };
        var command = new UpdateHeroCommand { Hero = hero };

        var repositoryMock = new Mock<ICassandraNeroRepository>();
        var handler = new UpdateHeroCommandHandler(repositoryMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        repositoryMock.Verify(r => r.UpdateAsync(It.Is<Hero>(h => h.Id == hero.Id && h.Name == hero.Name)), Times.Once);
    }
}
