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
        // GET: api/Profiles/{profileId}/Workouts
        [Authorize]
        [HttpGet("{profileId}/Workouts")]
        public async Task<ActionResult<IEnumerable<DateTime>>> GetWorkouts(int profileId)
        {
            // Retrieve the profile along with its workouts
            var workoutsDtaTime = await _context.Workouts
                                        .Where(p => p.ProfileId == profileId)
                                        .Select(w=> w.WorkoutDateTime)
                                        .ToListAsync();

            if (workoutsDtaTime.Count == 0)
            {
                return NotFound(new { Message = "No workouts were found" });
            }

            return workoutsDtaTime;
        }

        // POST: api/Profile/Workouts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Profile>> PostWorkout(DateTime workoutDateTime, int profileId)
        {
            var profile = _context.Profiles.FirstOrDefault(p => p.ProfileId == profileId);
            if (profile == null)
            {
                return NotFound();
            }

            _context.Workouts.Add(new Workout {
                ProfileId = profileId,
                WorkoutDateTime = workoutDateTime,
                Profile = profile,
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfile", new { id = profile.ProfileId }, profile);
        }

        // POST: api/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Profile>> PostProfile(Profile profile)
        {
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfile", new { id = profile.ProfileId }, profile);
        }

        // DELETE: api/Profiles/5
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

        private bool ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.ProfileId == id);
        }
    }
}
