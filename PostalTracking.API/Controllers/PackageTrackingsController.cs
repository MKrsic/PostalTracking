using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostalTracking.DAL.Entities;

namespace PostalTracking.API.Controllers
{
    [Produces("application/json")]
    [Route("api/PackageTrackings")]
    public class PackageTrackingsController : Controller
    {
        private readonly PostalTrackingContext _context;

        public PackageTrackingsController(PostalTrackingContext context)
        {
            _context = context;
        }

        // GET: api/PackageTrackings
        /// <summary>
        /// Gets a list of all package trackings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<PackageTracking> GetPackageTracking()
        {
            return _context.PackageTracking;
        }

        // GET: api/PackageTrackings/5
        /// <summary>
        /// Gets a package track with enetered ID if exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageTracking([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var packageTracking = await _context.PackageTracking.SingleOrDefaultAsync(m => m.Id == id);

            if (packageTracking == null)
            {
                return NotFound();
            }

            return Ok(packageTracking);
        }

        // PUT: api/PackageTrackings/5
        /// <summary>
        /// Updates existing package track
        /// </summary>
        /// <param name="id"></param>
        /// <param name="packageTracking"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPackageTracking([FromRoute] int id, [FromBody] PackageTracking packageTracking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != packageTracking.Id)
            {
                return BadRequest();
            }

            _context.Entry(packageTracking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PackageTrackingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(packageTracking);
        }

        // POST: api/PackageTrackings
        /// <summary>
        /// Inserts new package tracking
        /// </summary>
        /// <param name="packageTracking"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostPackageTracking([FromBody] PackageTracking packageTracking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PackageTracking.Add(packageTracking);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PackageTrackingExists(packageTracking.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPackageTracking", new { id = packageTracking.Id }, packageTracking);
        }

        // DELETE: api/PackageTrackings/5
        /// <summary>
        /// Deletes package track with entered ID if exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackageTracking([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var packageTracking = await _context.PackageTracking.SingleOrDefaultAsync(m => m.Id == id);
            if (packageTracking == null)
            {
                return NotFound();
            }

            _context.PackageTracking.Remove(packageTracking);
            await _context.SaveChangesAsync();

            return Ok(packageTracking);
        }

        private bool PackageTrackingExists(int id)
        {
            return _context.PackageTracking.Any(e => e.Id == id);
        }
    }
}