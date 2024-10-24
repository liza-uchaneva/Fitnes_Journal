using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness_Journal.Data
{
    public class Profile 
    {
        [Key]
        public int ProfileId { get; set; }

        public List<Workout> Workouts { get; set; }
        public Profile()
        {
            Workouts = new List<Workout>();
        }
    }
}
