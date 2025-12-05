namespace LP_app.Models;

public class Quiz
{
    public int Id { get; set; }
    public int LessonId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TimeLimit { get; set; } // in minutes
    public decimal PassingScore { get; set; } = 70m; // percentage
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign keys and navigation
    public Lesson? Lesson { get; set; }
    public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
    public ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
}
