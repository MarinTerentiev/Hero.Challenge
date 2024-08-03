namespace Domain.Common;

public class RabbitmqSettings
{
    public required string ConnectionStrings { get; set; }
    public string? HostName { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

