using ECinema.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteTicketFromShoppingCart(string userId, Guid ticketId);
        bool order(string userId);
    }
}
