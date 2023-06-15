using ECinema.Domain.Relations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECinema.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {
        [Required]
        public string movieGenre { get; set; }

        [Required]
        public string movieName { get; set; }

        [Required]
        public string movieImage { get; set; }

        [Required]
        public DateTime validDate { get; set; }

        [Required]
        public double ticketPrice { get; set; }


        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCart { get; set; }
        public virtual ICollection<TicketInOrder> TicketInOrder { get; set; }

    }
}
