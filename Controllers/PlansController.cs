using ClosetMuseBackend.Models;
using ClosetMuseBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClosetMuseBackend.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/plans")]
    public class PlansController : ControllerBase
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlans(string userId)
        {
            var plans = await _planService.GetAllPlansAsync(userId);
            return Ok(plans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlan(string userId, string id)
        {
            var plan = await _planService.GetPlanByIdAsync(userId, id);
            return plan is not null ? Ok(plan) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddPlan(string userId, [FromBody] Plan plan)
        {
            await _planService.AddPlanAsync(userId, plan);
            return CreatedAtAction(nameof(GetPlan), new { userId, id = plan.Id }, plan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlan(string userId, string id, [FromBody] Plan plan)
        {
            plan.Id = id;
            await _planService.UpdatePlanAsync(userId, plan);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan(string userId, string id)
        {
            await _planService.DeletePlanAsync(userId, id);
            return NoContent();
        }
    }
}
