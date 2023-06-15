using ECinema.Domain.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<TicketInShoppingCart> Tickets { get; set; }
        public double TotalPrice { get; set; }
    }
}
