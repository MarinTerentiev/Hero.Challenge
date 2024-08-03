using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Receiver.Consumers;
using Receiver.Models;
using System.Net;

namespace HeroTestUnit;

public class HeroRabbitmqConsumerTests
{
    private readonly Mock<IOptions<RabbitmqSettings>> _mockRabbitmqOptions;
    private readonly Mock<IOptions<BaseApiSettings>> _mockApiOptions;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;

    public HeroRabbitmqConsumerTests()
    {
        _mockRabbitmqOptions = new Mock<IOptions<RabbitmqSettings>>();
        _mockApiOptions = new Mock<IOptions<BaseApiSettings>>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

        _mockRabbitmqOptions.Setup(x => x.Value).Returns(new RabbitmqSettings { ConnectionStrings = "amqp://localhost" });
        _mockApiOptions.Setup(x => x.Value).Returns(new BaseApiSettings { Url = "http://localhost/" });
    }

    [Fact]
    public async Task HandleMessageAsync_ShouldReturnTrue_WhenMessageIsProcessedSuccessfully()
    {
        // Arrange
        var hero = new Hero { Id = Guid.NewGuid(), Name = "Test Hero", Class = "Warrior", Story = "Some story", Weapon = Weapon.Sword };
        var heroJson = JsonConvert.SerializeObject(hero);

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var consumer = new HeroRabbitmqConsumer(_mockRabbitmqOptions.Object, _mockApiOptions.Object, _httpClient);

        // Act
        var result = await consumer.TestHandleMessageAsync(heroJson, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task HandleMessageAsync_ShouldReturnFalse_WhenMessageProcessingFails()
    {
        // Arrange
        var heroJson = "{\"InvalidJson\": \"This will fail deserialization\"}";
        var consumer = new HeroRabbitmqConsumer(_mockRabbitmqOptions.Object, _mockApiOptions.Object, _httpClient);

        // Act
        var result = await consumer.TestHandleMessageAsync(heroJson, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task HandleMessageAsync_ShouldReturnFalse_WhenHttpRequestFails()
    {
        // Arrange
        var hero = new Hero { Id = Guid.NewGuid(), Name = "Test Hero", Class = "Warrior", Story = "Some story", Weapon = Weapon.Sword };
        var heroJson = JsonConvert.SerializeObject(hero);

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

        var consumer = new HeroRabbitmqConsumer(_mockRabbitmqOptions.Object, _mockApiOptions.Object, _httpClient);

        // Act
        var result = await consumer.TestHandleMessageAsync(heroJson, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}
