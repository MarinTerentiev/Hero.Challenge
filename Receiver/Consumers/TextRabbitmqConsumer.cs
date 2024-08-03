using Domain.Common;
using Microsoft.Extensions.Options;

namespace Receiver.Consumers;

public class TextRabbitmqConsumer : BaseRabbitmqConsumer
{
    public TextRabbitmqConsumer(IOptions<RabbitmqSettings> options) : base(options, RabbitmqQueues.TextQueur)
    {

    }

    protected override async Task<bool> HandleMessageAsync(string message, CancellationToken stoppingToken)
    {
        await Task.CompletedTask;
        return true;
    }

#if DEBUG
    public async Task<bool> TestHandleMessageAsync(string message, CancellationToken stoppingToken)
    {
        return await HandleMessageAsync(message, stoppingToken);
    }
#endif
}
