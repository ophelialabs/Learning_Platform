using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LP_app.Data;
using LP_app.Models;
using LP_app.Dtos;

namespace LP_app.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificatesController : ControllerBase
{
    private readonly LearningPlatformContext _context;
    private readonly ILogger<CertificatesController> _logger;

    public CertificatesController(LearningPlatformContext context, ILogger<CertificatesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>Get all certificates for a user</summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<CertificateDto>>> GetUserCertificates(int userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = $"User with ID {userId} not found" });

            var certificates = await _context.Certificates
                .Where(c => c.UserId == userId)
                .Include(c => c.Course)
                .Select(c => new CertificateDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    CourseId = c.CourseId,
                    CourseName = c.Course != null ? c.Course.Title : "Unknown Course",
                    VerificationCode = c.VerificationCode,
                    IssuedDate = c.IssuedDate,
                    ExpiryDate = c.ExpiryDate,
                    Status = c.Status
                })
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} certificates for user {UserId}", 
                certificates.Count, userId);

            return Ok(certificates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving certificates for user {UserId}", userId);
            return StatusCode(500, new { message = "Error retrieving certificates", error = ex.Message });
        }
    }

    /// <summary>Get certificate details by ID</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CertificateDetailDto>> GetCertificate(int id)
    {
        try
        {
            var certificate = await _context.Certificates
                .Include(c => c.User)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (certificate == null)
                return NotFound(new { message = $"Certificate with ID {id} not found" });

            var certificateDetailDto = new CertificateDetailDto
            {
                Id = certificate.Id,
                UserName = certificate.User != null ? $"{certificate.User.FirstName} {certificate.User.LastName}" : "Unknown",
                UserEmail = certificate.User?.Email ?? "Unknown",
                CourseName = certificate.Course?.Title ?? "Unknown Course",
                VerificationCode = certificate.VerificationCode,
                IssuedDate = certificate.IssuedDate,
                ExpiryDate = certificate.ExpiryDate,
                Status = certificate.Status
            };

            return Ok(certificateDetailDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving certificate {CertificateId}", id);
            return StatusCode(500, new { message = "Error retrieving certificate", error = ex.Message });
        }
    }

    /// <summary>Generate certificate for user upon course completion</summary>
    [HttpPost("generate")]
    public async Task<ActionResult<CertificateDto>> GenerateCertificate([FromBody] GenerateCertificateDto generateDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verify user exists
            var user = await _context.Users.FindAsync(generateDto.UserId);
            if (user == null)
                return NotFound(new { message = $"User with ID {generateDto.UserId} not found" });

            // Verify course exists
            var course = await _context.Courses.FindAsync(generateDto.CourseId);
            if (course == null)
                return NotFound(new { message = $"Course with ID {generateDto.CourseId} not found" });

            // Check if user is enrolled in the course
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == generateDto.UserId && e.CourseId == generateDto.CourseId);

            if (enrollment == null)
            {
                _logger.LogWarning("User {UserId} not enrolled in course {CourseId}", 
                    generateDto.UserId, generateDto.CourseId);
                return BadRequest(new { message = "User is not enrolled in this course" });
            }

            // Check if course is completed (completion >= 100%)
            if (enrollment.CompletionPercentage < 100)
            {
                _logger.LogWarning(
                    "User {UserId} has not completed course {CourseId} (Progress: {Progress}%)",
                    generateDto.UserId, generateDto.CourseId, enrollment.CompletionPercentage);
                return BadRequest(new 
                { 
                    message = "User has not completed the course",
                    progress = enrollment.CompletionPercentage
                });
            }

            // Check if certificate already exists
            var existingCertificate = await _context.Certificates
                .FirstOrDefaultAsync(c => c.UserId == generateDto.UserId && c.CourseId == generateDto.CourseId);

            if (existingCertificate != null)
            {
                _logger.LogWarning("Certificate already exists for user {UserId} and course {CourseId}",
                    generateDto.UserId, generateDto.CourseId);
                return BadRequest(new { message = "Certificate has already been issued for this user and course" });
            }

            // Generate verification code
            string verificationCode = GenerateVerificationCode();

            // Create certificate
            var certificate = new Certificate
            {
                UserId = generateDto.UserId,
                CourseId = generateDto.CourseId,
                VerificationCode = verificationCode,
                IssuedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(2),
                Status = "Active"
            };

            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Certificate generated for user {UserId} on course {CourseId} with code {VerificationCode}",
                generateDto.UserId, generateDto.CourseId, verificationCode);

            var certificateDto = new CertificateDto
            {
                Id = certificate.Id,
                UserId = certificate.UserId,
                CourseId = certificate.CourseId,
                CourseName = course.Title,
                VerificationCode = certificate.VerificationCode,
                IssuedDate = certificate.IssuedDate,
                ExpiryDate = certificate.ExpiryDate,
                Status = certificate.Status
            };

            return CreatedAtAction(nameof(GetCertificate), new { id = certificate.Id }, certificateDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating certificate");
            return StatusCode(500, new { message = "Error generating certificate", error = ex.Message });
        }
    }

    /// <summary>Verify certificate authenticity</summary>
    [HttpPost("verify")]
    public async Task<ActionResult<CertificateVerificationDto>> VerifyCertificate(
        [FromBody] CertificateVerificationDto verificationDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(verificationDto.VerificationCode))
                return BadRequest(new { message = "Verification code is required" });

            var certificate = await _context.Certificates
                .Include(c => c.User)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.VerificationCode == verificationDto.VerificationCode);

            var result = new CertificateVerificationDto
            {
                VerificationCode = verificationDto.VerificationCode,
                IsValid = certificate != null && certificate.Status == "Active" && 
                          (certificate.ExpiryDate == null || certificate.ExpiryDate > DateTime.UtcNow)
            };

            if (result.IsValid && certificate != null)
            {
                result.Certificate = new CertificateDetailDto
                {
                    Id = certificate.Id,
                    UserName = certificate.User != null ? $"{certificate.User.FirstName} {certificate.User.LastName}" : "Unknown",
                    UserEmail = certificate.User?.Email ?? "Unknown",
                    CourseName = certificate.Course?.Title ?? "Unknown Course",
                    VerificationCode = certificate.VerificationCode,
                    IssuedDate = certificate.IssuedDate,
                    ExpiryDate = certificate.ExpiryDate,
                    Status = certificate.Status
                };

                _logger.LogInformation("Certificate {VerificationCode} verified successfully", 
                    verificationDto.VerificationCode);
            }
            else
            {
                _logger.LogInformation("Certificate verification failed for code {VerificationCode}", 
                    verificationDto.VerificationCode);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying certificate");
            return StatusCode(500, new { message = "Error verifying certificate", error = ex.Message });
        }
    }

    /// <summary>Get all certificates for a course</summary>
    [HttpGet("course/{courseId}")]
    public async Task<ActionResult<IEnumerable<CertificateDto>>> GetCourseCertificates(int courseId)
    {
        try
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                return NotFound(new { message = $"Course with ID {courseId} not found" });

            var certificates = await _context.Certificates
                .Where(c => c.CourseId == courseId)
                .Include(c => c.User)
                .Select(c => new CertificateDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    CourseId = c.CourseId,
                    CourseName = course.Title,
                    VerificationCode = c.VerificationCode,
                    IssuedDate = c.IssuedDate,
                    ExpiryDate = c.ExpiryDate,
                    Status = c.Status
                })
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} certificates for course {CourseId}", 
                certificates.Count, courseId);

            return Ok(certificates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving certificates for course {CourseId}", courseId);
            return StatusCode(500, new { message = "Error retrieving certificates", error = ex.Message });
        }
    }

    /// <summary>Revoke a certificate</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> RevokeCertificate(int id)
    {
        try
        {
            var certificate = await _context.Certificates.FindAsync(id);

            if (certificate == null)
                return NotFound(new { message = $"Certificate with ID {id} not found" });

            certificate.Status = "Revoked";
            _context.Certificates.Update(certificate);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Certificate {CertificateId} has been revoked", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking certificate {CertificateId}", id);
            return StatusCode(500, new { message = "Error revoking certificate", error = ex.Message });
        }
    }

    /// <summary>Generate a unique verification code</summary>
    private string GenerateVerificationCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var result = new string(Enumerable.Range(0, 12)
            .Select(_ => chars[random.Next(chars.Length)])
            .ToArray());
        return $"CERT-{DateTime.UtcNow:yyyyMMdd}-{result}";
    }
}
