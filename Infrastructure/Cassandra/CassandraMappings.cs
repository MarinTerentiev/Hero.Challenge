using Domain.Entities;

namespace Infrastructure.CassandraRepository;

public class CassandraMappings : Cassandra.Mapping.Mappings
{
    public CassandraMappings()
    {
        For<Hero>()
            .TableName("hero")
            .PartitionKey(x => x.Id)
            .Column(h => h.Id, cm => cm.WithName("id"))
            .Column(h => h.Name, cm => cm.WithName("name"))
            .Column(h => h.Class, cm => cm.WithName("class"))
            .Column(h => h.Story, cm => cm.WithName("story"))
            .Column(h => h.Weapon, cm => cm.WithName("weapon").WithDbType<int>());
    }
}
