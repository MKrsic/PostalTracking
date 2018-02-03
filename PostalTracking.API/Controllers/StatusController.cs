using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostalTracking.DAL.Entities;

namespace PostalTracking.API.Controllers
{
    [ApiVersion("1.0"), ApiVersion("1.1")]
    [Produces("application/json")]
    [Route("api/Status")]
    public class StatusController : Controller
    {
        private readonly PostalTrackingContext _context;

        public StatusController(PostalTrackingContext context)
        {
            _context = context;
        }

        // GET: api/Status
        /// <summary>
        /// Gets a list of all statuses
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Status> GetStatus()
        {
            return _context.Status;
        }

        // GET: api/Status/5
        /// <summary>
        /// Gets a status with enetered ID if exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStatus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var status = await _context.Status.SingleOrDefaultAsync(m => m.Id == id);

            if (status == null)
            {
                return NotFound();
            }

            return Ok(status);
        }

        // PUT: api/Status/5
        /// <summary>
        /// Updates existing status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatus([FromRoute] int id, [FromBody] Status status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != status.Id)
            {
                return BadRequest();
            }

            _context.Entry(status).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Status
        /// <summary>
        /// Inserts new status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostStatus([FromBody] Status status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Status.Add(status);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStatus", new { id = status.Id }, status);
        }

        // DELETE: api/Status/5
        /// <summary>
        /// Deletes status with entered ID if exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var status = await _context.Status.SingleOrDefaultAsync(m => m.Id == id);
            if (status == null)
            {
                return NotFound();
            }

            _context.Status.Remove(status);
            await _context.SaveChangesAsync();

            return Ok(status);
        }

        private bool StatusExists(int id)
        {
            return _context.Status.Any(e => e.Id == id);
        }
    }
}