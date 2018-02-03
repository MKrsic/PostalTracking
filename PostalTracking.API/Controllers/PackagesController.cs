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
    [Route("api/Packages")]
    public class PackagesController : Controller
    {
        private readonly PostalTrackingContext _context;

        public PackagesController(PostalTrackingContext context)
        {
            _context = context;
        }

        // GET: api/Packages
        /// <summary>
        /// Gets a list of all packages
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Package> GetPackage()
        {
            return _context.Package;
        }

        // GET: api/Packages/5
        /// <summary>
        /// Gets a package with enetered ID if exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var package = await _context.Package.SingleOrDefaultAsync(m => m.Id == id);

            if (package == null)
            {
                return NotFound();
            }

            return Ok(package);
        }

        // PUT: api/Packages/5
        /// <summary>
        /// Updates existing package
        /// </summary>
        /// <param name="id"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPackage([FromRoute] int id, [FromBody] Package package)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != package.Id)
            {
                return BadRequest();
            }

            _context.Entry(package).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PackageExists(id))
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

        // POST: api/Packages
        /// <summary>
        /// Inserts new package
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostPackage([FromBody] Package package)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Package.Add(package);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPackage", new { id = package.Id }, package);
        }

        // DELETE: api/Packages/5
        /// <summary>
        /// Deletes package with entered ID if exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var package = await _context.Package.SingleOrDefaultAsync(m => m.Id == id);
            if (package == null)
            {
                return NotFound();
            }

            _context.Package.Remove(package);
            await _context.SaveChangesAsync();

            return Ok(package);
        }

        private bool PackageExists(int id)
        {
            return _context.Package.Any(e => e.Id == id);
        }
    }
}