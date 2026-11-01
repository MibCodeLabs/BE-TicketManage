using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ticketManage.DTOs.Ticket;
using ticketManage.Services;
using ticketManage.Utils;

namespace ticketManage.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ODataController
    {
        private TicketService _ticketService;

        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public async Task<IActionResult> UpsertTicket(UpsertTicketRequest request)
        {
            return Ok(await _ticketService.UpsertTicket(request));
        }

        [HttpGet]
        [EnableQuery(PageSize = Constants.DEFAULT_MAX_PAGE_SIZE)]
        public IQueryable<TicketResponse> Get()
        {
            return _ticketService.GetTickets();
        }
    }
}
