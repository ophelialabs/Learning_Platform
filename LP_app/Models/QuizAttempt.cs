namespace LP_app.Models;

public class QuizAttempt
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int QuizId { get; set; }
    public decimal Score { get; set; } // percentage
    public bool Passed { get; set; } = false;
    public DateTime AttemptDate { get; set; } = DateTime.UtcNow;
    public int TimeSpent { get; set; } // in seconds

    // Foreign keys
    public User? User { get; set; }
    public Quiz? Quiz { get; set; }
}
