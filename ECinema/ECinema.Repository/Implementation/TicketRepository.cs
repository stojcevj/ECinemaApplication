using ECinema.Domain.DomainModels;
using ECinema.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Repository.Implementation
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Ticket> entities;
        string errorMessage = string.Empty;

        public TicketRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Ticket>();
        }
        public List<Ticket> GetAllTicketsFromDate(DateTime date)
        {
            var items = entities.ToListAsync().Result;
            List<Ticket> returnList = new List<Ticket>();

            foreach(Ticket item in items)
            {
                if(item.validDate.Date == date.Date)
                {
                    returnList.Add(item);
                }
            }

            return returnList;
        }

        public List<Ticket> GetAllTicketsFromGenre(string genre)
        {
            return entities.ToListAsync()
                .Result
                .FindAll(z => z.movieGenre == genre);
        }
    }
}
