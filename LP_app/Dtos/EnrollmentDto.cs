namespace LP_app.Dtos;

/// <summary>Enrollment Data Transfer Objects</summary>
/// 
public class EnrollmentDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrolledDate { get; set; }
    public decimal CompletionPercentage { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? CompletedDate { get; set; }
}

public class CreateEnrollmentDto
{
    public int UserId { get; set; }
    public int CourseId { get; set; }
}

public class UpdateProgressDto
{
    public decimal CompletionPercentage { get; set; }
    public string? Status { get; set; }
}
