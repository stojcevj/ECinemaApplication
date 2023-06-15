using ECinema.Domain.DomainModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECinema.Domain.Identity
{
    public class ECinemaApplicationUser : IdentityUser
    {
        public virtual ShoppingCart UserCart { get; set; }
    }
}

/*
 * admin@admin.com
 * Admin123!
 */
