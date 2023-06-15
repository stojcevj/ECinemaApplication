using ECinema.Domain;
using ECinema.Domain.DomainModels;
using ECinema.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Repository.Implementation
{
     public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }
        public List<Order> getAllOrders()
        {
            return entities
                .Include(z => z.User)
                .Include(z => z.TicketInOrder)
                .Include("TicketInOrder.Ticket")
                .ToListAsync().Result;
        }

        public List<Order> getAllOrdersForUser(string userId)
        {
            return entities
                .Include(z => z.User)
                .Include(z => z.TicketInOrder)
                .Include("TicketInOrder.Ticket")
                .ToListAsync()
                .Result
                .FindAll(z => z.UserId == userId);
        }

        public Order getOrderDetails(BaseEntity model)
        {
            return entities
               .Include(z => z.User)
               .Include(z => z.TicketInOrder)
               .Include("TicketInOrder.Ticket")
               .SingleOrDefaultAsync(z => z.Id == model.Id).Result;
        }
    }
}
