namespace Application.RabbitmqPublisher;

public interface ITextPublisher
{
    Task Publishe(string text);
}
