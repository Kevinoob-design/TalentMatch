using Microsoft.EntityFrameworkCore;
using TalentMatch.Models;

namespace TalentMatch.Data;

public class TalentMatchContext : DbContext
{
    public TalentMatchContext(DbContextOptions<TalentMatchContext> options) : base(options)
    {
    }

    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<JobPosition> JobPositions => Set<JobPosition>();
    public DbSet<Application> Applications => Set<Application>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Candidate configuration
        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Email).IsUnique();
            entity.Property(c => c.Status).HasConversion<string>();
        });

        // JobPosition configuration
        modelBuilder.Entity<JobPosition>(entity =>
        {
            entity.HasKey(j => j.Id);
            entity.Property(j => j.Status).HasConversion<string>();
        });

        // Application configuration (join table)
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Status).HasConversion<string>();

            // Relationships
            entity.HasOne(a => a.Candidate)
                .WithMany(c => c.Applications)
                .HasForeignKey(a => a.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(a => a.JobPosition)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobPositionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prevent duplicate applications
            entity.HasIndex(a => new { a.CandidateId, a.JobPositionId }).IsUnique();
        });
    }
}
