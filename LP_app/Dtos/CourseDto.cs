namespace LP_app.Dtos;

/// <summary>Course Data Transfer Objects</summary>

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public int DurationHours { get; set; }
    public int LessonCount { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Instructor { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateCourseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public int DurationHours { get; set; }
    public string Level { get; set; } = "Beginner";
    public string Instructor { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
}

public class UpdateCourseDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Duration { get; set; }
    public int? DurationHours { get; set; }
    public string? Level { get; set; }
    public string? Instructor { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Price { get; set; }
}
