using Application.HeroImportPostgressComponent.Queries;
using Application.RabbitmqPublisher;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using WebAPI.Controllers;

namespace HeroTestUnit;

public class HeroRabbitmqControllerTests
{
    private readonly Mock<IHeroPublisher> _heroPublisherMock;
    private readonly Mock<ITextPublisher> _textPublisherMock;
    private readonly HeroRabbitmqController _controller;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<HeroRabbitmqController>> _logger;

    public HeroRabbitmqControllerTests()
    {
        _heroPublisherMock = new Mock<IHeroPublisher>();
        _textPublisherMock = new Mock<ITextPublisher>();
        _mediatorMock = new Mock<IMediator>();
        _logger = new Mock<ILogger<HeroRabbitmqController>>();
        _controller = new HeroRabbitmqController(_heroPublisherMock.Object, _textPublisherMock.Object, _mediatorMock.Object, _logger.Object);
    }

    [Fact]
    public async Task SendText_CallsPublisheMethod()
    {
        // Arrange
        _textPublisherMock.Setup(p => p.Publishe(It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        await _controller.SendText();

        // Assert
        _textPublisherMock.Verify(p => p.Publishe("test text"), Times.Once);
    }

    [Fact]
    public async Task BulkUploadHero_ReturnsFailedRows_WhenFileIsInvalid()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var content = "Name,Class,Story,Weapon\nHero1,Warrior,Story1,Sword\nHero2,Archer,Story2,Bow";
        var fileName = "test.csv";
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
        ms.Position = 0;

        fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(ms.Length);

        var formFileCollection = new FormFileCollection { fileMock.Object };
        var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(), formFileCollection);

        var httpContextMock = new DefaultHttpContext();
        httpContextMock.Request.Form = formCollection;

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = httpContextMock
        };

        _heroPublisherMock.Setup(p => p.Publishe(It.IsAny<Hero>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.BulkUploadHero();

        // Assert
        Assert.Empty(result);
        _heroPublisherMock.Verify(p => p.Publishe(It.IsAny<Hero>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Get_ReturnsOkResult_WithListOfHeroes()
    {
        // Arrange
        var seedId = Guid.NewGuid();
        var heroes = new List<HeroImport>
        {
            new HeroImport { Id = 1, Name = "Hero1", Class = "Warrior", Story = "Story1", Weapon = Domain.Enums.Weapon.Sword },
            new HeroImport { Id = 2, Name = "Hero2", Class = "Mage", Story = "Story2", Weapon = Domain.Enums.Weapon.Spear }
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetBySeedIdHeroImportQuery>(q => q.SeedId == seedId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(heroes);

        // Act
        var result = await _controller.Get(seedId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<HeroImport>>(okResult.Value);
        Assert.Equal(heroes, returnValue);
    }
}
