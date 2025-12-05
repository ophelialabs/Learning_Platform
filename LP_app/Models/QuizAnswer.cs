namespace LP_app.Models;

public class QuizAnswer
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Answer { get; set; } = string.Empty;
    public bool IsCorrect { get; set; } = false;
    public int AnswerOrder { get; set; }

    // Foreign keys
    public QuizQuestion? Question { get; set; }
}
