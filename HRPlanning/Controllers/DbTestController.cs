using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRPlanning.Data;

namespace HRPlanning.Controllers
{
    [ApiController]
    [Route("api/db")]
    public class DbTestController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public DbTestController(ApplicationDbContext db) => _db = db;

        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            try
            {
                var can = await _db.Database.CanConnectAsync();
                return Ok(new { canConnect = can });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { canConnect = false, error = ex.Message });
            }
        }
    }
}