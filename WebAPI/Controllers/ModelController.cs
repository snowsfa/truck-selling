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
    public class ModelController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModelController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Models
        [HttpGet]
        public IEnumerable<Model> GetModels()
        {
            return _context.Models;
        }

        // GET: api/Models/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await _context.Models.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        // PUT: api/Models/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModel([FromRoute] int id, [FromBody] Model model)
        {
            model.Description = model.Description.Trim();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.ModelId)
            {
                return BadRequest();
            }

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!ModelExists(id))
                {
                    return NotFound();
                }
                else if (TrunkModelConflicts(model))
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

        // POST: api/Models
        [HttpPost]
        public async Task<IActionResult> PostModel([FromBody] Model model)
        {
            model.ModelId = 0;
            model.Description = model.Description.Trim();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Models.Add(model);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ModelExists(model.Description))
                {
                    return Conflict();
                }
                throw ex;
            }

            return CreatedAtAction("GetModel", new { id = model.ModelId }, model);
        }

        // DELETE: api/Models/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            _context.Models.Remove(model);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (HasVinculatedTruck(model))
                {
                    return UnprocessableEntity();
                }
                throw ex;
            }
            
            return Ok(model);
        }

        private bool ModelExists(int id)
        {
            /* using .Count instead of .Any as a workaround for a bug on casting TINYINT(1) to BOOLEAN */
            return (_context.Models.Where(e => e.ModelId == id).Count() > 0);
        }

        private bool ModelExists(string description)
        {
            return (_context.Models.Where(e => e.Description == description).Count() > 0);
        }

        private bool TrunkModelConflicts(Model model)
        {
            return (_context.Models.Where(e => e.Description == model.Description && e.ModelId != model.ModelId).Count() > 0);
        }

        private bool HasVinculatedTruck(Model model)
        {
            return (_context.Trucks.Where(t => t.Model.ModelId == model.ModelId).Count() > 0);
        }
    }
}