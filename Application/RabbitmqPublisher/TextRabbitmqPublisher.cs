using Domain.Common;
using Microsoft.Extensions.Options;
using System.Text;

namespace Application.RabbitmqPublisher;

public class TextRabbitmqPublisher : BaseRabbitmqPublisher<string>, ITextPublisher 
{
    public TextRabbitmqPublisher(IOptions<RabbitmqSettings> options) : base(options, RabbitmqQueues.TextQueur)
    {

    }

    protected override byte[] SerializeMessage(string message)
    {
        return Encoding.UTF8.GetBytes(message);
    }
}
