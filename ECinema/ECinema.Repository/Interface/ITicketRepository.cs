using ECinema.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Repository.Interface
{
    public interface ITicketRepository
    {
        List<Ticket> GetAllTicketsFromDate(DateTime date);
        List<Ticket> GetAllTicketsFromGenre(string genre);
    }
}
