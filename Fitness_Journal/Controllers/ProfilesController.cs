using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fitness_Journal.Data;
using Microsoft.AspNetCore.Authorization;

namespace Fitness_Journal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Profile>> PostProfile()
        {
            var profile = new Profile();
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
            var result = new ProfileCreateResult() { ProfiletId = profile.ProfileId };
            return CreatedAtAction("GetProfile", result);
        }

        // DELETE: api/Profiles/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

    public class ProfileCreateResult
    {
        public int ProfiletId { get; set; }
    }
}
