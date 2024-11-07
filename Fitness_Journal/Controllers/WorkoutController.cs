using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fitness_Journal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Net.Mime.MediaTypeNames;

namespace Fitness_Journal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkouController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkouController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Workout
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Profile>> PostWorkout(CreateWorkoutModel workoutmodel)
        {
            var profile = await _context.Profiles.FindAsync(workoutmodel.ProfileId);
            if (profile == null)
            {
                return NotFound("Profile not found.");
            }

            if (await WorkoutExistsAsync(workoutmodel.WorkoutDateTime, workoutmodel.ProfileId))
            {
                return Conflict("A workout with the specified date and time already exists.");
            }
            var workout = new Workout
            {
                Profile = profile,
                ProfileId = workoutmodel.ProfileId,
                WorkoutDateTime = workoutmodel.WorkoutDateTime,
            };

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetWorkout",
                new WorkoutCreateResult()
                {
                    WorkoutId = workout.WorkoutId,
                    WorkoutDateTime = workoutmodel.WorkoutDateTime
                });
        }
        // GET: api/profileId/workouts
        [Authorize]
        [HttpGet("/profileId/workouts")]
        public async Task<ActionResult<IEnumerable<DateTime>>> GetWorkoutsFromUser(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if(profile == null) return NotFound("Profile id not found.");

            var workoutDates = profile.Workouts.Select(x => x.WorkoutDateTime);
            return Ok(workoutDates);
        }
        private async Task<bool> WorkoutExistsAsync(DateTime workoutDateTime, int profileId)
        {
            return await _context.Workouts
                .AnyAsync(w => w.WorkoutDateTime == workoutDateTime && w.ProfileId == profileId);
        }
    }

    public class CreateWorkoutModel
    {
        public int ProfileId { get; set; }

        public DateTime WorkoutDateTime { get; set; }

    }

    public class WorkoutCreateResult
    {
        public int WorkoutId { get; set; }
        public DateTime WorkoutDateTime { get; set; }
    }
}
