using ECinema.Domain;
using ECinema.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Repository.Interface
{
    public interface IOrderRepository
    {
        public List<Order> getAllOrders();
        public Order getOrderDetails(BaseEntity model);
        public List<Order> getAllOrdersForUser(string userId);
    }
}
