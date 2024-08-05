using Domain.Common;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Receiver.Models;

namespace Receiver.Consumers;

public class HeroRabbitmqConsumer : BaseRabbitmqConsumer
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly ILogger<HeroRabbitmqConsumer> _logger;

    public HeroRabbitmqConsumer(IOptions<RabbitmqSettings> options, IOptions<BaseApiSettings> apiOptions, HttpClient httpClient,
        ILogger<HeroRabbitmqConsumer> logger)
        : base(options, RabbitmqQueues.HeroUploadQueur, logger)
    {
        _httpClient = httpClient;
        _baseUrl = apiOptions.Value.Url.ToString();
        _logger = logger;
    }

    protected override async Task<bool> HandleMessageAsync(string message, CancellationToken stoppingToken)
    {
        try
        {
            var hero = JsonConvert.DeserializeObject<Hero>(message);
            var content = JsonContent.Create(hero);
            var url = $"{_baseUrl}api/HeroCassandra";
            var response = await _httpClient.PostAsync(url, content);

            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            var error = $"Error processing message: {ex.Message}";
            _logger.LogError(ex, error);

            return false;
        }
    }

#if DEBUG
    public Task<bool> TestHandleMessageAsync(string message, CancellationToken stoppingToken)
    {
        return HandleMessageAsync(message, stoppingToken);
    }
#endif
}
