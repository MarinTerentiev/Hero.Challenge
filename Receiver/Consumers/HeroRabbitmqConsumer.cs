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

    public HeroRabbitmqConsumer(IOptions<RabbitmqSettings> options, IOptions<BaseApiSettings> apiOptions, HttpClient httpClient)
        : base(options, RabbitmqQueues.HeroUploadQueur)
    {
        _httpClient = httpClient;
        _baseUrl = apiOptions.Value.Url.ToString();
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
