namespace LP_app.Dtos;

/// <summary>Quiz Data Transfer Objects</summary>
/// 
public class QuizDto
{
    public int Id { get; set; }
    public int LessonId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TimeLimit { get; set; }
    public decimal PassingScore { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class QuizQuestionDto
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public int QuestionOrder { get; set; }
    public List<QuizAnswerDto> Answers { get; set; } = new();
}

public class QuizAnswerDto
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Answer { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int AnswerOrder { get; set; }
}

public class QuizDetailDto
{
    public int Id { get; set; }
    public int LessonId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TimeLimit { get; set; }
    public decimal PassingScore { get; set; }
    public List<QuizQuestionDto> Questions { get; set; } = new();
}

public class QuizSubmissionDto
{
    public List<QuestionAnswerDto> Answers { get; set; } = new();
}

public class QuestionAnswerDto
{
    public int QuestionId { get; set; }
    public int SelectedAnswerId { get; set; }
}

public class QuizResultDto
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public decimal Score { get; set; }
    public DateTime AttemptDate { get; set; }
    public int TimeSpent { get; set; }
    public bool Passed { get; set; }
}
