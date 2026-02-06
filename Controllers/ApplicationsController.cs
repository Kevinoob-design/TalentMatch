using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentMatch.Data;
using TalentMatch.Models;

namespace TalentMatch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly TalentMatchContext _context;

    public ApplicationsController(TalentMatchContext context)
    {
        _context = context;
    }

    // GET: api/applications
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
    {
        return await _context.Applications
            .Include(a => a.Candidate)
            .Include(a => a.JobPosition)
            .ToListAsync();
    }

    // GET: api/applications/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Application>> GetApplication(int id)
    {
        var application = await _context.Applications
            .Include(a => a.Candidate)
            .Include(a => a.JobPosition)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (application == null)
        {
            return NotFound();
        }

        return application;
    }

    // GET: api/applications/candidate/5
    [HttpGet("candidate/{candidateId}")]
    public async Task<ActionResult<IEnumerable<Application>>> GetApplicationsByCandidate(int candidateId)
    {
        return await _context.Applications
            .Include(a => a.JobPosition)
            .Where(a => a.CandidateId == candidateId)
            .ToListAsync();
    }

    // GET: api/applications/job/5
    [HttpGet("job/{jobId}")]
    public async Task<ActionResult<IEnumerable<Application>>> GetApplicationsByJob(int jobId)
    {
        return await _context.Applications
            .Include(a => a.Candidate)
            .Where(a => a.JobPositionId == jobId)
            .ToListAsync();
    }

    // POST: api/applications
    [HttpPost]
    public async Task<ActionResult<Application>> CreateApplication(Application application)
    {
        // Verify candidate exists
        var candidateExists = await _context.Candidates.AnyAsync(c => c.Id == application.CandidateId);
        if (!candidateExists)
        {
            return BadRequest("Candidate not found");
        }

        // Verify job position exists
        var jobExists = await _context.JobPositions.AnyAsync(j => j.Id == application.JobPositionId);
        if (!jobExists)
        {
            return BadRequest("Job position not found");
        }

        // Check if application already exists
        var existingApplication = await _context.Applications
            .AnyAsync(a => a.CandidateId == application.CandidateId && a.JobPositionId == application.JobPositionId);
        if (existingApplication)
        {
            return Conflict("Application already exists for this candidate and job position");
        }

        application.AppliedAt = DateTime.UtcNow;
        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, application);
    }

    // PUT: api/applications/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApplication(int id, Application application)
    {
        if (id != application.Id)
        {
            return BadRequest();
        }

        var existingApplication = await _context.Applications.FindAsync(id);
        if (existingApplication == null)
        {
            return NotFound();
        }

        existingApplication.Status = application.Status;
        existingApplication.Notes = application.Notes;
        existingApplication.CoverLetter = application.CoverLetter;
        existingApplication.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/applications/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApplication(int id)
    {
        var application = await _context.Applications.FindAsync(id);
        if (application == null)
        {
            return NotFound();
        }

        _context.Applications.Remove(application);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
