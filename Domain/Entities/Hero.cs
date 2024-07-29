using Domain.Enums;

namespace Domain.Entities;

public class Hero
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Class { get; set; } = "";
    public string Story { get; set; } = "";
    public Weapon Weapon { get; set; } = Weapon.None;
}
