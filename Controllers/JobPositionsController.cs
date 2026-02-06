using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentMatch.Data;
using TalentMatch.Models;

namespace TalentMatch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobPositionsController : ControllerBase
{
    private readonly TalentMatchContext _context;

    public JobPositionsController(TalentMatchContext context)
    {
        _context = context;
    }

    // GET: api/jobpositions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobPosition>>> GetJobPositions()
    {
        return await _context.JobPositions
            .Include(j => j.Applications)
            .ToListAsync();
    }

    // GET: api/jobpositions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<JobPosition>> GetJobPosition(int id)
    {
        var jobPosition = await _context.JobPositions
            .Include(j => j.Applications)
                .ThenInclude(a => a.Candidate)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (jobPosition == null)
        {
            return NotFound();
        }

        return jobPosition;
    }

    // POST: api/jobpositions
    [HttpPost]
    public async Task<ActionResult<JobPosition>> CreateJobPosition(JobPosition jobPosition)
    {
        jobPosition.CreatedAt = DateTime.UtcNow;
        _context.JobPositions.Add(jobPosition);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetJobPosition), new { id = jobPosition.Id }, jobPosition);
    }

    // PUT: api/jobpositions/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJobPosition(int id, JobPosition jobPosition)
    {
        if (id != jobPosition.Id)
        {
            return BadRequest();
        }

        var existingJob = await _context.JobPositions.FindAsync(id);
        if (existingJob == null)
        {
            return NotFound();
        }

        existingJob.Title = jobPosition.Title;
        existingJob.Description = jobPosition.Description;
        existingJob.Requirements = jobPosition.Requirements;
        existingJob.MinSalary = jobPosition.MinSalary;
        existingJob.MaxSalary = jobPosition.MaxSalary;
        existingJob.Location = jobPosition.Location;
        existingJob.Status = jobPosition.Status;
        existingJob.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/jobpositions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJobPosition(int id)
    {
        var jobPosition = await _context.JobPositions.FindAsync(id);
        if (jobPosition == null)
        {
            return NotFound();
        }

        _context.JobPositions.Remove(jobPosition);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
