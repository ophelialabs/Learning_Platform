namespace LP_app.Dtos;

/// <summary>Certificate Data Transfer Objects</summary>
/// 
public class CertificateDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string VerificationCode { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CertificateDetailDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string VerificationCode { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class GenerateCertificateDto
{
    public int UserId { get; set; }
    public int CourseId { get; set; }
}

public class CertificateVerificationDto
{
    public string VerificationCode { get; set; } = string.Empty;
    public bool IsValid { get; set; }
    public CertificateDetailDto? Certificate { get; set; }
}
