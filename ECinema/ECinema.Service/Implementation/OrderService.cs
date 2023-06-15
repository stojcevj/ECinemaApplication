using ECinema.Domain;
using ECinema.Domain.DomainModels;
using ECinema.Repository.Interface;
using ECinema.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public List<Order> getAllOrders()
        {
            return this._orderRepository.getAllOrders();
        }

        public List<Order> getAllOrdersForUser(string userId)
        {
            return this._orderRepository.getAllOrdersForUser(userId);
        }

        public Order getOrderDetails(BaseEntity model)
        {
            return this._orderRepository.getOrderDetails(model);
        }
    }
}
