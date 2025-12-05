namespace LP_app.Models;

public class Enrollment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrolledDate { get; set; } = DateTime.UtcNow;
    public decimal CompletionPercentage { get; set; } = 0m;
    public string Status { get; set; } = "In Progress"; // In Progress, Completed, Paused
    public DateTime? CompletedDate { get; set; }

    // Foreign keys
    public User? User { get; set; }
    public Course? Course { get; set; }
}
