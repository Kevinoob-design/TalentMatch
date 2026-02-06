using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentMatch.Data;
using TalentMatch.Models;

namespace TalentMatch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidatesController : ControllerBase
{
    private readonly TalentMatchContext _context;

    public CandidatesController(TalentMatchContext context)
    {
        _context = context;
    }

    // GET: api/candidates
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidates()
    {
        return await _context.Candidates
            .Include(c => c.Applications)
            .ToListAsync();
    }

    // GET: api/candidates/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Candidate>> GetCandidate(int id)
    {
        var candidate = await _context.Candidates
            .Include(c => c.Applications)
                .ThenInclude(a => a.JobPosition)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (candidate == null)
        {
            return NotFound();
        }

        return candidate;
    }

    // POST: api/candidates
    [HttpPost]
    public async Task<ActionResult<Candidate>> CreateCandidate(Candidate candidate)
    {
        candidate.CreatedAt = DateTime.UtcNow;
        _context.Candidates.Add(candidate);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCandidate), new { id = candidate.Id }, candidate);
    }

    // PUT: api/candidates/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCandidate(int id, Candidate candidate)
    {
        if (id != candidate.Id)
        {
            return BadRequest();
        }

        var existingCandidate = await _context.Candidates.FindAsync(id);
        if (existingCandidate == null)
        {
            return NotFound();
        }

        existingCandidate.Name = candidate.Name;
        existingCandidate.Email = candidate.Email;
        existingCandidate.Phone = candidate.Phone;
        existingCandidate.Skills = candidate.Skills;
        existingCandidate.YearsOfExperience = candidate.YearsOfExperience;
        existingCandidate.Status = candidate.Status;
        existingCandidate.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/candidates/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCandidate(int id)
    {
        var candidate = await _context.Candidates.FindAsync(id);
        if (candidate == null)
        {
            return NotFound();
        }

        _context.Candidates.Remove(candidate);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
