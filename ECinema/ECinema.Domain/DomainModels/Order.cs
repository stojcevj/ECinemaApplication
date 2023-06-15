using ECinema.Domain.Identity;
using ECinema.Domain.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public ECinemaApplicationUser User { get; set; }

        public virtual ICollection<TicketInOrder> TicketInOrder { get; set; }
    }
}
