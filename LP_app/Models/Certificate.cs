namespace LP_app.Models;

public class Certificate
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime IssuedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiryDate { get; set; }
    public string Status { get; set; } = "Active"; // Active, Expired, Revoked
    public string VerificationCode { get; set; } = Guid.NewGuid().ToString();
    public DateTime? ValidUntil { get; set; }

    // Foreign keys
    public User? User { get; set; }
    public Course? Course { get; set; }
}
