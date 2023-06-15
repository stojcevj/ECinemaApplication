using ECinema.Domain.DomainModels;
using ECinema.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Service.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAllTickets();
        Ticket GetDetailsForTicket(Guid? id);
        void CreateNewTicket(Ticket p);
        void UpdateExistingTicket(Ticket p);
        AddToShoppingCartDto GetShoppingCartInfo(Guid? id);
        void DeleteTicket(Guid id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userID);
        List<Ticket> GetAllTicketsFromDate(DateTime date);
        List<Ticket> GetAllTicketsWithGenre(string genre);
    }
}
