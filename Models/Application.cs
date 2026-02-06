using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TalentMatch.Enums;

namespace TalentMatch.Models;

public class Application
{
    public int Id { get; set; }

    public int CandidateId { get; set; }

    public int JobPositionId { get; set; }

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

    [MaxLength(1000)]
    public string? Notes { get; set; }

    [MaxLength(500)]
    public string? CoverLetter { get; set; }

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Candidate? Candidate { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JobPosition? JobPosition { get; set; }
}
