using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TalentMatch.Enums;

namespace TalentMatch.Models;

public class Candidate
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(500)]
    public string? Skills { get; set; }

    public int YearsOfExperience { get; set; }

    public CandidateStatus Status { get; set; } = CandidateStatus.Active;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<Application>? Applications { get; set; }
}
