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
        // GET: api/user/name
        [Authorize]
        [HttpGet("/user/name")]
        public async Task<ActionResult<int>> GetUserName()
        {
            var IdentUser = await _userManager.GetUserAsync(User);
            if (IdentUser != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == IdentUser.Email);
                if(user !=null)
                {
                    return Ok(user.UserName);
                }
                else
                {
                    return NotFound("User id not found.");
                }
            }
            else
            {
                return NotFound("Profile not found.");
            }
        }
        // GET: api/user/profileId
        [Authorize]
        [HttpGet("/user/profileId")]
        public async Task<ActionResult<int>> GetProfileId()
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

        // GET: api/user/weeklygoal
        [Authorize]
        [HttpGet("/user/weeklygoal")]
        public async Task<ActionResult<int>> GetWeeklyGoal()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("User id not found.");

            var profile = _context.Profiles.FirstOrDefault(p => p.ProfileId == user.ProfileId);
            if (profile == null) return NotFound("Profile not found.");

            return Ok(profile.WeeklyGoal);
        }

        // POST: api/user/weeklygoal
        [Authorize]
        [HttpPost("/user/setweeklygoal")]
        public async Task<IActionResult> PostWeeklyGoal(CreateProfileWeeklyGoal createProfileWeeklyGoal)
        {
            var profile = _context.Profiles.FirstOrDefault(p => p.ProfileId == createProfileWeeklyGoal.ProfileId);
            if (profile == null) return NotFound("Profile not found.");

            profile.WeeklyGoal = createProfileWeeklyGoal.WeeklyGoal;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                ProfileId = profile.ProfileId,
                Goal = profile.WeeklyGoal
            });
        }

        // POST: api/user/workout
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("/user/workout")]
        public async Task<ActionResult<WorkoutCreateResult>> PostWorkout(CreateWorkoutModel createWorkoutModel)
        {
            var workoutModel = createWorkoutModel;

            var profile = await _context.Profiles.FindAsync(workoutModel.ProfileId);
            if (profile == null)
            {
                return NotFound("Profile not found.");
            }

            if (await WorkoutExistsAsync(workoutModel.WorkoutDateTime, workoutModel.ProfileId))
            {
                return Conflict("A workout with the specified date and time already exists.");
            }

            var workout = new Workout
            {
                Profile = profile,
                ProfileId = workoutModel.ProfileId,
                WorkoutDateTime = workoutModel.WorkoutDateTime,
            };

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();

            return new WorkoutCreateResult()
            {
                WorkoutId = workout.WorkoutId,
                WorkoutDateTime = workoutModel.WorkoutDateTime
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
