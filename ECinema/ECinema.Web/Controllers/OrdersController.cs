using ECinema.Domain.Identity;
using ECinema.Service.Interface;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECinema.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ECinemaApplicationUser> _userManager;

        public OrdersController(IOrderService orderService, UserManager<ECinemaApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier.ToString()).Value;

            return View(_orderService.getAllOrdersForUser(userId));
        }

        public IActionResult CreateInvoice(Guid id)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            var order = _orderService.getOrderDetails(new Domain.BaseEntity { Id = id });
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", order.Id.ToString());
            document.Content.Replace("{{CostumerEmail}}", order.User.Email);
            document.Content.Replace("{{CostumerInfo}}", (order.User.UserName + " " + order.User.PhoneNumber));

            StringBuilder sb = new StringBuilder();

            var total = 0.0;

            foreach (var item in order.TicketInOrder)
            {
                total += item.Quantity * item.Ticket.ticketPrice;
                sb.AppendLine(item.Ticket.movieName + " with quantity of: " + item.Quantity + " and price of: $" + item.Ticket.ticketPrice);
            }

            document.Content.Replace("{{AllTickets}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", "$" + total.ToString());

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());
            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }
    }
}
