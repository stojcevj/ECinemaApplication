using ECinema.Domain.Identity;
using ECinema.Domain.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public virtual ECinemaApplicationUser Owner { get; set; }

        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCart { get; set; }
    }
}
