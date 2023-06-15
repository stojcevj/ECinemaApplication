using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Domain.DTO
{
    public class UserImportDto
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NormalizedUserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
