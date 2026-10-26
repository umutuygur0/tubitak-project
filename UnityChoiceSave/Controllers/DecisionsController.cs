using Microsoft.AspNetCore.Mvc;
using UnityChoiceSave.DataAccess;
using UnityChoiceSave.Models;

namespace UnityChoiceSave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DecisionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DecisionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveDecision([FromBody] DecisionLog log)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            log.CreatedAt = DateTime.UtcNow;
            await _context.DecisionLogs.AddAsync(log);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Kaydedildi", id = log.Id });
        }
    }
}