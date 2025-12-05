using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LP_app.Data;
using LP_app.Models;
using LP_app.Dtos;

namespace LP_app.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly LearningPlatformContext _context;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(LearningPlatformContext context, ILogger<CoursesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>Get all courses with pagination</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var courses = await _context.Courses
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Category = c.Category,
                    Duration = c.Duration,
                    DurationHours = c.DurationHours,
                    LessonCount = c.LessonCount,
                    Level = c.Level,
                    Instructor = c.Instructor,
                    ImageUrl = c.ImageUrl,
                    Price = c.Price,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} courses (page {PageNumber}, size {PageSize})", 
                courses.Count, pageNumber, pageSize);

            return Ok(courses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving courses");
            return StatusCode(500, new { message = "Error retrieving courses", error = ex.Message });
        }
    }

    /// <summary>Get course by ID</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourse(int id)
    {
        try
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                _logger.LogWarning("Course with ID {CourseId} not found", id);
                return NotFound(new { message = $"Course with ID {id} not found" });
            }

            var courseDto = new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Category = course.Category,
                Duration = course.Duration,
                DurationHours = course.DurationHours,
                LessonCount = course.LessonCount,
                Level = course.Level,
                Instructor = course.Instructor,
                ImageUrl = course.ImageUrl,
                Price = course.Price,
                CreatedAt = course.CreatedAt
            };

            return Ok(courseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving course {CourseId}", id);
            return StatusCode(500, new { message = "Error retrieving course", error = ex.Message });
        }
    }

    /// <summary>Create a new course</summary>
    [HttpPost]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = new Course
            {
                Title = createCourseDto.Title,
                Description = createCourseDto.Description,
                Category = createCourseDto.Category,
                Duration = createCourseDto.Duration,
                DurationHours = createCourseDto.DurationHours,
                Level = createCourseDto.Level,
                Instructor = createCourseDto.Instructor,
                ImageUrl = createCourseDto.ImageUrl,
                Price = createCourseDto.Price,
                CreatedAt = DateTime.UtcNow
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Course created successfully with ID {CourseId}", course.Id);

            var courseDto = new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Category = course.Category,
                Duration = course.Duration,
                DurationHours = course.DurationHours,
                LessonCount = course.LessonCount,
                Level = course.Level,
                Instructor = course.Instructor,
                ImageUrl = course.ImageUrl,
                Price = course.Price,
                CreatedAt = course.CreatedAt
            };

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, courseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating course");
            return StatusCode(500, new { message = "Error creating course", error = ex.Message });
        }
    }

    /// <summary>Update an existing course</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseDto updateCourseDto)
    {
        try
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                _logger.LogWarning("Course with ID {CourseId} not found for update", id);
                return NotFound(new { message = $"Course with ID {id} not found" });
            }

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(updateCourseDto.Title))
                course.Title = updateCourseDto.Title;

            if (!string.IsNullOrWhiteSpace(updateCourseDto.Description))
                course.Description = updateCourseDto.Description;

            if (!string.IsNullOrWhiteSpace(updateCourseDto.Category))
                course.Category = updateCourseDto.Category;

            if (!string.IsNullOrWhiteSpace(updateCourseDto.Duration))
                course.Duration = updateCourseDto.Duration;

            if (updateCourseDto.DurationHours.HasValue)
                course.DurationHours = updateCourseDto.DurationHours.Value;

            if (!string.IsNullOrWhiteSpace(updateCourseDto.Level))
                course.Level = updateCourseDto.Level;

            if (!string.IsNullOrWhiteSpace(updateCourseDto.Instructor))
                course.Instructor = updateCourseDto.Instructor;

            if (!string.IsNullOrWhiteSpace(updateCourseDto.ImageUrl))
                course.ImageUrl = updateCourseDto.ImageUrl;

            if (updateCourseDto.Price.HasValue)
                course.Price = updateCourseDto.Price.Value;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Course with ID {CourseId} updated successfully", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating course {CourseId}", id);
            return StatusCode(500, new { message = "Error updating course", error = ex.Message });
        }
    }

    /// <summary>Delete a course</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        try
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                _logger.LogWarning("Course with ID {CourseId} not found for deletion", id);
                return NotFound(new { message = $"Course with ID {id} not found" });
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Course with ID {CourseId} deleted successfully", id);
            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Cannot delete course {CourseId} - has related data", id);
            return BadRequest(new 
            { 
                message = "Cannot delete course - it has related enrollments or lessons", 
                error = ex.InnerException?.Message 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting course {CourseId}", id);
            return StatusCode(500, new { message = "Error deleting course", error = ex.Message });
        }
    }

    /// <summary>Get courses by category</summary>
    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesByCategory(string category)
    {
        try
        {
            var courses = await _context.Courses
                .Where(c => c.Category == category)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Category = c.Category,
                    Duration = c.Duration,
                    DurationHours = c.DurationHours,
                    LessonCount = c.LessonCount,
                    Level = c.Level,
                    Instructor = c.Instructor,
                    ImageUrl = c.ImageUrl,
                    Price = c.Price,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            if (!courses.Any())
            {
                _logger.LogInformation("No courses found for category {Category}", category);
                return Ok(new List<CourseDto>());
            }

            _logger.LogInformation("Retrieved {Count} courses for category {Category}", 
                courses.Count, category);

            return Ok(courses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving courses by category {Category}", category);
            return StatusCode(500, new { message = "Error retrieving courses", error = ex.Message });
        }
    }
}
