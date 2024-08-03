using Domain.Common;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Application.RabbitmqPublisher;

public class HeroRabbitmqPublisher : BaseRabbitmqPublisher<Hero>, IHeroPublisher
{
    public HeroRabbitmqPublisher(IOptions<RabbitmqSettings> options) : base(options, RabbitmqQueues.HeroUploadQueur)
    {

    }

    protected override byte[] SerializeMessage(Hero message)
    {
        var json = JsonConvert.SerializeObject(message);
        return Encoding.UTF8.GetBytes(json);
    }
}
