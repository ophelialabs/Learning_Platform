namespace LP_app.Models;

public class UserAchievement
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AchievementId { get; set; }
    public DateTime EarnedDate { get; set; } = DateTime.UtcNow;

    // Foreign keys
    public User? User { get; set; }
    public Achievement? Achievement { get; set; }
}
