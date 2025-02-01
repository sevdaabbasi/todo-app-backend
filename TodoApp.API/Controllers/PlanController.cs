using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;
using TodoApp.Core.DTOs.Plan;
using TodoApp.Core.Exceptions;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

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
    public async Task<IActionResult> CreatePlan([FromBody] CreatePlanDto dto)
    {
        var plan = new Plan
        {
            Title = dto.Title,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            PlanType = dto.PlanType,
            UserId = int.Parse(User.Identity?.Name ?? "0")
        };
        
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
    public async Task<IActionResult> UpdatePlan(int id, [FromBody] UpdatePlanDto dto)
    {
        try 
        {
            var existingPlan = await _planService.GetPlanByIdAsync(id);
            if (existingPlan == null)
                return NotFound();

        
            existingPlan.Title = dto.Title;
            existingPlan.Description = dto.Description;
            existingPlan.StartDate = dto.StartDate;
            existingPlan.EndDate = dto.EndDate;
            existingPlan.PlanType = dto.PlanType;

            await _planService.UpdatePlanAsync(existingPlan);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
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
        try
        {
            await _planService.AddCollaboratorAsync(id, userId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while adding collaborator" });
        }
    }

    [HttpDelete("{id}/collaborators/{userId}")]
    public async Task<IActionResult> RemoveCollaborator(int id, int userId)
    {
        await _planService.RemoveCollaboratorAsync(id, userId);
        return NoContent();
    }
} 