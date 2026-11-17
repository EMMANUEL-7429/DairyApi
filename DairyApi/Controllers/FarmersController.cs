using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DairyApi.Data;
using DairyApi.Models;

namespace DairyApi.Controllers
{
    [ApiController]
    [Route("api/farmers")]
    public class FarmersController : ControllerBase
    {
        private readonly DairyDbContext _context;

        public FarmersController(DairyDbContext context)
        {
            _context = context;
        }

        // GET: api/farmers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Farmer>>> GetFarmers()
        {
            // Include milk deliveries when returning farmers
            var farmers = await _context.Farmers
                .Include(f => f.MilkDeliveries)
                .ToListAsync();

            return Ok(farmers);
        }

        // GET: api/farmers/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Farmer>> GetFarmer(int id)
        {
            var farmer = await _context.Farmers
                .Include(f => f.MilkDeliveries)
                .FirstOrDefaultAsync(f => f.FarmerId == id);

            if (farmer == null) return NotFound();

            return Ok(farmer);
        }

        // POST: api/farmers
        [HttpPost]
        public async Task<ActionResult> CreateFarmer(Farmer farmer)
        {
            farmer.CreatedAt = DateTime.UtcNow;

            foreach (var delivery in farmer.MilkDeliveries)
            {
                delivery.CreatedAt = DateTime.UtcNow;
                // Farmer navigation removed, no need to set anything
            }

            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFarmer),
                new { id = farmer.FarmerId }, farmer);
        }

        // PUT: api/farmers/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateFarmer(int id, Farmer farmer)
        {
            if (id != farmer.FarmerId)
                return BadRequest();

            _context.Entry(farmer).State = EntityState.Modified;

            // Update createdAt for new deliveries if needed
            foreach (var delivery in farmer.MilkDeliveries)
            {
                if (delivery.CreatedAt == default)
                    delivery.CreatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/farmers/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteFarmer(int id)
        {
            var farmer = await _context.Farmers
                .Include(f => f.MilkDeliveries)
                .FirstOrDefaultAsync(f => f.FarmerId == id);

            if (farmer == null) return NotFound();

            _context.Farmers.Remove(farmer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
