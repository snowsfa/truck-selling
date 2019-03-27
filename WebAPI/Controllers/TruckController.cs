using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruckSelling.WebAPI.Models;

namespace TruckSelling.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TruckController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Truck
        [HttpGet]
        public IEnumerable<Truck> GetTrucks()
        {
            return _context.Trucks.Include(t => t.Model);
        }

        // GET: api/Truck/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTruck([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var truck = await _context.Trucks.Include(t => t.Model).FirstOrDefaultAsync(t => t.TruckId == id);
            if (truck == null)
            {
                return NotFound();
            }

            return Ok(truck);
        }

        // PUT: api/Truck/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTruck([FromRoute] int id, [FromBody] Truck truck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != truck.TruckId)
            {
                return BadRequest();
            }

            // validate if model exists
            truck.Model = await _context.Models.FindAsync(truck.Model.ModelId);
            if (truck.Model == null)
            {
                return NotFound();
            }            

            _context.Entry(truck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!TruckExists(id))
                {
                    return NotFound();
                }
                else if (TrunkConflicts(truck))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Truck
        [HttpPost]
        public async Task<IActionResult> PostTruck([FromBody] Truck truck)
        {
            truck.TruckId = 0;
            truck.RegistrationDate = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // validate if model exists
            truck.Model = await _context.Models.FindAsync(truck.Model.ModelId);
            if (truck.Model == null)
            {
                return NotFound();
            }

            _context.Trucks.Add(truck);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (TruckExists(truck.Chassis))
                {
                    return Conflict();
                }
                throw ex;
            }

            return CreatedAtAction("GetTruck", new { id = truck.TruckId }, truck);
        }

        // DELETE: api/Truck/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTruck([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var truck = await _context.Trucks.Include(t => t.Model).FirstOrDefaultAsync(t => t.TruckId == id);
            if (truck == null)
            {
                return NotFound();
            }

            _context.Trucks.Remove(truck);
            await _context.SaveChangesAsync();

            return Ok(truck);
        }

        private bool TruckExists(int id)
        {
            /* using .Count instead of .Any as a workaround for a bug on casting TINYINT(1) to BOOLEAN */
            return (_context.Trucks.Where(e => e.TruckId == id).Count() > 0);
        }

        private bool TruckExists(string chassis)
        {
            return (_context.Trucks.Where(e => e.Chassis == chassis).Count() > 0);
        }

        private bool TrunkConflicts(Truck truck)
        {
            return (_context.Trucks.Where(e => e.Chassis == truck.Chassis && e.TruckId != truck.TruckId).Count() > 0);
        }
    }
}