namespace LP_app.Models;

public class Achievement
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string Criteria { get; set; } = string.Empty; // JSON or description of criteria

    // Navigation
    public ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}
