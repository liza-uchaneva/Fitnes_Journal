using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Fitness_Journal.Data
{
    public class User : IdentityUser
    {
        public int? ProfileId { get; set; }
        public Profile? Profile { get; set; }

        public User()
        {
            Profile = new Profile();
        }

    }
}
