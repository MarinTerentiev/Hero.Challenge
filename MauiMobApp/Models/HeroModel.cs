namespace MauiMobApp.Models;

public class HeroModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Class { get; set; } = "";
    public string Story { get; set; } = "";
    public Weapon Weapon { get; set; }
}
