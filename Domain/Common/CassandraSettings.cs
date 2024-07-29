namespace Domain.Common;

public class CassandraSettings
{
    public string ContactPoints { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Keyspace { get; set; } = string.Empty;
}
