namespace Domain.Common;

public class CassandraSettings
{
    public string ContactPoints { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Keyspace { get; set; }
}
