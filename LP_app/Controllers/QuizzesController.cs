using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LP_app.Data;
using LP_app.Models;
using LP_app.Dtos;

namespace LP_app.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    private readonly LearningPlatformContext _context;
    private readonly ILogger<QuizzesController> _logger;

    public QuizzesController(LearningPlatformContext context, ILogger<QuizzesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>Get quiz by ID with all questions and answers</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<QuizDetailDto>> GetQuiz(int id)
    {
        try
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
                return NotFound(new { message = $"Quiz with ID {id} not found" });

            var quizDetailDto = new QuizDetailDto
            {
                Id = quiz.Id,
                LessonId = quiz.LessonId,
                Title = quiz.Title,
                Description = quiz.Description,
                TimeLimit = quiz.TimeLimit,
                PassingScore = quiz.PassingScore,
                Questions = quiz.Questions.OrderBy(q => q.QuestionOrder).Select(q => new QuizQuestionDto
                {
                    Id = q.Id,
                    QuizId = q.QuizId,
                    Question = q.Question,
                    QuestionType = q.QuestionType,
                    QuestionOrder = q.QuestionOrder,
                    Answers = q.Answers.OrderBy(a => a.AnswerOrder).Select(a => new QuizAnswerDto
                    {
                        Id = a.Id,
                        QuestionId = a.QuestionId,
                        Answer = a.Answer,
                        IsCorrect = a.IsCorrect,
                        AnswerOrder = a.AnswerOrder
                    }).ToList()
                }).ToList()
            };

            _logger.LogInformation("Retrieved quiz {QuizId} with {QuestionCount} questions", 
                id, quizDetailDto.Questions.Count);

            return Ok(quizDetailDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving quiz {QuizId}", id);
            return StatusCode(500, new { message = "Error retrieving quiz", error = ex.Message });
        }
    }

    /// <summary>Get quizzes for a specific lesson</summary>
    [HttpGet("lesson/{lessonId}")]
    public async Task<ActionResult<IEnumerable<QuizDto>>> GetLessonQuizzes(int lessonId)
    {
        try
        {
            var lesson = await _context.Lessons.FindAsync(lessonId);
            if (lesson == null)
                return NotFound(new { message = $"Lesson with ID {lessonId} not found" });

            var quizzes = await _context.Quizzes
                .Where(q => q.LessonId == lessonId)
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    LessonId = q.LessonId,
                    Title = q.Title,
                    Description = q.Description,
                    TimeLimit = q.TimeLimit,
                    PassingScore = q.PassingScore,
                    CreatedAt = q.CreatedAt
                })
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} quizzes for lesson {LessonId}", 
                quizzes.Count, lessonId);

            return Ok(quizzes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving quizzes for lesson {LessonId}", lessonId);
            return StatusCode(500, new { message = "Error retrieving quizzes", error = ex.Message });
        }
    }

    /// <summary>Submit quiz answers and get score</summary>
    [HttpPost("{id}/submit")]
    public async Task<ActionResult<QuizResultDto>> SubmitQuiz(int id, [FromBody] QuizSubmissionDto submissionDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
                return NotFound(new { message = $"Quiz with ID {id} not found" });

            // Calculate score
            int correctAnswers = 0;
            int totalQuestions = quiz.Questions.Count;

            foreach (var answer in submissionDto.Answers)
            {
                var question = quiz.Questions.FirstOrDefault(q => q.Id == answer.QuestionId);
                if (question == null)
                    continue;

                var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == answer.SelectedAnswerId);
                if (selectedAnswer?.IsCorrect == true)
                    correctAnswers++;
            }

            decimal scorePercentage = totalQuestions > 0 
                ? (correctAnswers * 100m) / totalQuestions 
                : 0;

            bool passed = scorePercentage >= quiz.PassingScore;

            // Log attempt - if user ID is provided in headers or claims, store it
            // For now, we'll log the attempt but note that UserId isn't in submission
            var quizAttempt = new QuizAttempt
            {
                QuizId = id,
                AttemptDate = DateTime.UtcNow,
                Score = scorePercentage,
                TimeSpent = 0,
                Passed = passed
            };

            _context.QuizAttempts.Add(quizAttempt);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Quiz {QuizId} submitted: Score={Score}%, Passed={Passed}, Correct={Correct}/{Total}",
                id, scorePercentage, passed, correctAnswers, totalQuestions);

            var resultDto = new QuizResultDto
            {
                Id = quizAttempt.Id,
                QuizId = quizAttempt.QuizId,
                Score = quizAttempt.Score,
                AttemptDate = quizAttempt.AttemptDate,
                TimeSpent = quizAttempt.TimeSpent,
                Passed = quizAttempt.Passed
            };

            return CreatedAtAction(nameof(GetQuizResults), 
                new { quizId = id }, resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting quiz {QuizId}", id);
            return StatusCode(500, new { message = "Error submitting quiz", error = ex.Message });
        }
    }

    /// <summary>Get all results for a specific quiz</summary>
    [HttpGet("{quizId}/results")]
    public async Task<ActionResult<IEnumerable<QuizResultDto>>> GetQuizResults(int quizId)
    {
        try
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz == null)
                return NotFound(new { message = $"Quiz with ID {quizId} not found" });

            var results = await _context.QuizAttempts
                .Where(qa => qa.QuizId == quizId)
                .OrderByDescending(qa => qa.AttemptDate)
                .Select(qa => new QuizResultDto
                {
                    Id = qa.Id,
                    QuizId = qa.QuizId,
                    Score = qa.Score,
                    AttemptDate = qa.AttemptDate,
                    TimeSpent = qa.TimeSpent,
                    Passed = qa.Passed
                })
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} results for quiz {QuizId}", 
                results.Count, quizId);

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving quiz results for quiz {QuizId}", quizId);
            return StatusCode(500, new { message = "Error retrieving results", error = ex.Message });
        }
    }

    /// <summary>Get quiz attempt details</summary>
    [HttpGet("attempts/{attemptId}")]
    public async Task<ActionResult<QuizResultDto>> GetQuizAttempt(int attemptId)
    {
        try
        {
            var attempt = await _context.QuizAttempts.FindAsync(attemptId);

            if (attempt == null)
                return NotFound(new { message = $"Quiz attempt with ID {attemptId} not found" });

            var resultDto = new QuizResultDto
            {
                Id = attempt.Id,
                QuizId = attempt.QuizId,
                Score = attempt.Score,
                AttemptDate = attempt.AttemptDate,
                TimeSpent = attempt.TimeSpent,
                Passed = attempt.Passed
            };

            return Ok(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving quiz attempt {AttemptId}", attemptId);
            return StatusCode(500, new { message = "Error retrieving attempt", error = ex.Message });
        }
    }

    /// <summary>Get all quizzes (paginated)</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizDto>>> GetAllQuizzes(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var quizzes = await _context.Quizzes
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    LessonId = q.LessonId,
                    Title = q.Title,
                    Description = q.Description,
                    TimeLimit = q.TimeLimit,
                    PassingScore = q.PassingScore,
                    CreatedAt = q.CreatedAt
                })
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} quizzes (page {PageNumber})", 
                quizzes.Count, pageNumber);

            return Ok(quizzes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving quizzes");
            return StatusCode(500, new { message = "Error retrieving quizzes", error = ex.Message });
        }
    }
}
