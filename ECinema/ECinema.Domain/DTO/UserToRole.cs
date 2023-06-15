using ECinema.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECinema.Domain.DTO
{
    public class UserToRole
    {
        public string Email { get; set; }
        public ICollection<ECinemaApplicationUser> userEmails { get; set; }
        public List<string> userRoles { get; set; }
        public string selectedRole { get; set; }

        public UserToRole()
        {
            userRoles = new List<string>();
            userEmails = new List<ECinemaApplicationUser>();
        }
    }
}
