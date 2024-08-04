using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class HeroImport : BaseEntity
{
    public string Name { get; set; } = "";
    public string Class { get; set; } = "";
    public string Story { get; set; } = "";
    public Weapon Weapon { get; set; } = Weapon.None;
    public Guid SeedId { get; set; }
}
