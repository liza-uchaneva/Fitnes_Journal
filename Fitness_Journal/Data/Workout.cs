using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness_Journal.Data
{
    public class Workout
    {
        [Key]
        public int WorkoutId { get; set; }

        public required int ProfileId { get; set; }

        public required Profile Profile { get; set; }

        public DateTime WorkoutDateTime { get; set; }
    }
}
