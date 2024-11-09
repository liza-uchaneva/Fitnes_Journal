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
using Microsoft.IdentityModel.Tokens;

namespace Fitness_Journal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UsersController(ApplicationDbContext context ,UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/user/profileId
        [Authorize]
        [HttpGet("/user/profileId")]
        public async Task<ActionResult<int>> GetProfileIdFromUser()
        {
            var IdentUser = await _userManager.GetUserAsync(User);
            if (IdentUser != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == IdentUser.Email);
                if(user !=null)
                {
                    return Ok(user.ProfileId);

                }
                else
                {
                    return NotFound("User id not found.");
                }
            }
            else
            {
                return NotFound("Profile id not found.");
            }
        }

        // POST: api/user/workout
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("/user/workout")]
        public async Task<ActionResult<WorkoutCreateResult>> PostWorkout(DateTime workoutDateTime)
        {
            var IdentUser = await _userManager.GetUserAsync(User);
            if (IdentUser == null) return NotFound("User id not found.");
            var user = _context.Users.FirstOrDefault(u => u.Email == IdentUser.Email);
            if (user == null) return NotFound("Profile not found.");
            var profileId = user.ProfileId;

            var workoutmodel = new CreateWorkoutModel()
            {
                ProfileId = profileId,
                WorkoutDateTime = workoutDateTime
            };

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

            return new WorkoutCreateResult()
            {
                WorkoutId = workout.WorkoutId,
                WorkoutDateTime = workoutmodel.WorkoutDateTime
            };
        }

        // GET: api/user/workouts
        [Authorize]
        [HttpGet("/user/workouts")]
        public async Task<ActionResult<IEnumerable<DateTime>>> GetWorkouts()
        {
            var IdentUser = await _userManager.GetUserAsync(User);
            if (IdentUser == null) return NotFound("User id not found.");
            var user = _context.Users.FirstOrDefault(u => u.Email == IdentUser.Email);
            if (user == null) return NotFound("Profile not found.");
            var profileId = user.ProfileId;

            var workoutDates = _context.Workouts.Where(x=>x.ProfileId == profileId).Select(x => x.WorkoutDateTime);
            return Ok(workoutDates);
        }

        private async Task<bool> WorkoutExistsAsync(DateTime? workoutDateTime, int? profileId)
        {
            return await _context.Workouts
                .AnyAsync(w => w.WorkoutDateTime == workoutDateTime && w.ProfileId == profileId);
        }
    }
}
