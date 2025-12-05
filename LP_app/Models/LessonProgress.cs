namespace LP_app.Models;

public class LessonProgress
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int LessonId { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime? CompletedDate { get; set; }
    public int WatchedDuration { get; set; } // in seconds

    // Foreign keys
    public User? User { get; set; }
    public Lesson? Lesson { get; set; }
}
