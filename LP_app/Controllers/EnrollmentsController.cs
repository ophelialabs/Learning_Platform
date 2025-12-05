using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LP_app.Data;
using LP_app.Models;
using LP_app.Dtos;

namespace LP_app.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly LearningPlatformContext _context;
    private readonly ILogger<EnrollmentsController> _logger;

    public EnrollmentsController(LearningPlatformContext context, ILogger<EnrollmentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>Enroll user in a course</summary>
    [HttpPost]
    public async Task<ActionResult<EnrollmentDto>> EnrollInCourse([FromBody] CreateEnrollmentDto enrollmentDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verify user exists
            var user = await _context.Users.FindAsync(enrollmentDto.UserId);
            if (user == null)
                return NotFound(new { message = $"User with ID {enrollmentDto.UserId} not found" });

            // Verify course exists
            var course = await _context.Courses.FindAsync(enrollmentDto.CourseId);
            if (course == null)
                return NotFound(new { message = $"Course with ID {enrollmentDto.CourseId} not found" });

            // Check if already enrolled
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == enrollmentDto.UserId && e.CourseId == enrollmentDto.CourseId);

            if (existingEnrollment != null)
            {
                _logger.LogWarning("User {UserId} already enrolled in course {CourseId}", 
                    enrollmentDto.UserId, enrollmentDto.CourseId);
                return BadRequest(new { message = "User is already enrolled in this course" });
            }

            var enrollment = new Enrollment
            {
                UserId = enrollmentDto.UserId,
                CourseId = enrollmentDto.CourseId,
                EnrolledDate = DateTime.UtcNow,
                CompletionPercentage = 0,
                Status = "Active"
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} enrolled in course {CourseId}", 
                enrollmentDto.UserId, enrollmentDto.CourseId);

            var enrollmentResponseDto = new EnrollmentDto
            {
                Id = enrollment.Id,
                UserId = enrollment.UserId,
                CourseId = enrollment.CourseId,
                EnrolledDate = enrollment.EnrolledDate,
                CompletionPercentage = enrollment.CompletionPercentage,
                Status = enrollment.Status
            };

            return CreatedAtAction(nameof(GetUserEnrollments), 
                new { userId = enrollment.UserId }, enrollmentResponseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enrolling user in course");
            return StatusCode(500, new { message = "Error enrolling in course", error = ex.Message });
        }
    }

    /// <summary>Get all enrollments for a user</summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetUserEnrollments(int userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = $"User with ID {userId} not found" });

            var enrollments = await _context.Enrollments
                .Where(e => e.UserId == userId)
                .Select(e => new EnrollmentDto
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    CourseId = e.CourseId,
                    EnrolledDate = e.EnrolledDate,
                    CompletionPercentage = e.CompletionPercentage,
                    Status = e.Status
                })
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} enrollments for user {UserId}", 
                enrollments.Count, userId);

            return Ok(enrollments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving enrollments for user {UserId}", userId);
            return StatusCode(500, new { message = "Error retrieving enrollments", error = ex.Message });
        }
    }

    /// <summary>Get a specific enrollment</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EnrollmentDto>> GetEnrollment(int id)
    {
        try
        {
            var enrollment = await _context.Enrollments.FindAsync(id);

            if (enrollment == null)
                return NotFound(new { message = $"Enrollment with ID {id} not found" });

            var enrollmentDto = new EnrollmentDto
            {
                Id = enrollment.Id,
                UserId = enrollment.UserId,
                CourseId = enrollment.CourseId,
                EnrolledDate = enrollment.EnrolledDate,
                CompletionPercentage = enrollment.CompletionPercentage,
                Status = enrollment.Status
            };

            return Ok(enrollmentDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving enrollment {EnrollmentId}", id);
            return StatusCode(500, new { message = "Error retrieving enrollment", error = ex.Message });
        }
    }

    /// <summary>Update enrollment progress</summary>
    [HttpPut("{id}/progress")]
    public async Task<IActionResult> UpdateProgress(int id, [FromBody] UpdateProgressDto progressDto)
    {
        try
        {
            var enrollment = await _context.Enrollments.FindAsync(id);

            if (enrollment == null)
                return NotFound(new { message = $"Enrollment with ID {id} not found" });

            // Validate progress percentage
            if (progressDto.CompletionPercentage < 0 || progressDto.CompletionPercentage > 100)
                return BadRequest(new { message = "Completion percentage must be between 0 and 100" });

            enrollment.CompletionPercentage = progressDto.CompletionPercentage;
            enrollment.Status = progressDto.Status ?? enrollment.Status;

            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Enrollment {EnrollmentId} progress updated to {Percentage}%", 
                id, progressDto.CompletionPercentage);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating enrollment progress {EnrollmentId}", id);
            return StatusCode(500, new { message = "Error updating progress", error = ex.Message });
        }
    }

    /// <summary>Unenroll user from a course</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> UnenrollFromCourse(int id)
    {
        try
        {
            var enrollment = await _context.Enrollments.FindAsync(id);

            if (enrollment == null)
                return NotFound(new { message = $"Enrollment with ID {id} not found" });

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User unenrolled from enrollment {EnrollmentId}", id);
            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Cannot delete enrollment {EnrollmentId} - has related data", id);
            return BadRequest(new 
            { 
                message = "Cannot unenroll - there are related lesson progress or quiz attempts",
                error = ex.InnerException?.Message 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unenrolling from course {EnrollmentId}", id);
            return StatusCode(500, new { message = "Error unenrolling from course", error = ex.Message });
        }
    }

    /// <summary>Get enrollments for a specific course</summary>
    [HttpGet("course/{courseId}")]
    public async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetCourseEnrollments(int courseId)
    {
        try
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                return NotFound(new { message = $"Course with ID {courseId} not found" });

            var enrollments = await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => new EnrollmentDto
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    CourseId = e.CourseId,
                    EnrolledDate = e.EnrolledDate,
                    CompletionPercentage = e.CompletionPercentage,
                    Status = e.Status
                })
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} enrollments for course {CourseId}", 
                enrollments.Count, courseId);

            return Ok(enrollments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving enrollments for course {CourseId}", courseId);
            return StatusCode(500, new { message = "Error retrieving course enrollments", error = ex.Message });
        }
    }
}
