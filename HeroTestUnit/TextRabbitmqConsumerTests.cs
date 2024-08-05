using Microsoft.Extensions.Options;
using Moq;
using Receiver.Consumers;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace HeroTestUnit;

public class TextRabbitmqConsumerTests
{
    private readonly Mock<ILogger<TextRabbitmqConsumer>> _logger;

    public TextRabbitmqConsumerTests()
    {
        _logger = new Mock<ILogger<TextRabbitmqConsumer>>();
    }

    [Fact]
    public async Task HandleMessageAsync_ReturnsTrue()
    {
        // Arrange
        var rabbitmqSettings = new RabbitmqSettings { ConnectionStrings = "amqp://guest:guest@localhost:5672" };
        var optionsMock = new Mock<IOptions<RabbitmqSettings>>();
        optionsMock.Setup(o => o.Value).Returns(rabbitmqSettings);

        var consumer = new TextRabbitmqConsumer(optionsMock.Object, _logger.Object);

        // Act
        var result = await consumer.TestHandleMessageAsync("Test message", CancellationToken.None);

        // Assert
        Assert.True(result);
    }
}
