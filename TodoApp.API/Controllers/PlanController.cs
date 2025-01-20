using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;

namespace TodoApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PlanController : ControllerBase
{
    private readonly IPlanService _planService;

    public PlanController(IPlanService planService)
    {
        _planService = planService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlan([FromBody] Plan plan)
    {
        var userId = int.Parse(User.Identity?.Name ?? "0");
        plan.UserId = userId;
        
        var createdPlan = await _planService.CreatePlanAsync(plan);
        return CreatedAtAction(nameof(GetPlanById), new { id = createdPlan.Id }, createdPlan);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlanById(int id)
    {
        var plan = await _planService.GetPlanByIdAsync(id);
        if (plan == null)
            return NotFound();

        return Ok(plan);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserPlans()
    {
        var userId = int.Parse(User.Identity?.Name ?? "0");
        var plans = await _planService.GetUserPlansAsync(userId);
        return Ok(plans);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActivePlans()
    {
        var userId = int.Parse(User.Identity?.Name ?? "0");
        var plans = await _planService.GetActivePlansAsync(userId);
        return Ok(plans);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlan(int id, [FromBody] Plan plan)
    {
        if (id != plan.Id)
            return BadRequest();

        await _planService.UpdatePlanAsync(plan);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlan(int id)
    {
        await _planService.DeletePlanAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/collaborators/{userId}")]
    public async Task<IActionResult> AddCollaborator(int id, int userId)
    {
        await _planService.AddCollaboratorAsync(id, userId);
        return NoContent();
    }

    [HttpDelete("{id}/collaborators/{userId}")]
    public async Task<IActionResult> RemoveCollaborator(int id, int userId)
    {
        await _planService.RemoveCollaboratorAsync(id, userId);
        return NoContent();
    }
} 