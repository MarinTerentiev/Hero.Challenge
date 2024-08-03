using Domain.Entities;

namespace Application.RabbitmqPublisher;

public interface IHeroPublisher
{
    Task Publishe(Hero hero);
}
