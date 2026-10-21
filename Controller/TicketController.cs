using Microsoft.AspNetCore.Mvc;
using ticketManage.DTOs.Ticket;
using ticketManage.Services;

namespace ticketManage.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private TicketService _ticketService;

        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(CreateTicketRequest request)
        {
            return Ok(await _ticketService.CreateTicket(request));
        }

        //[HttpGet]
        //public async Task<IActionResult> getTickets(Long id)
        //{
        //    return Ok(await _ticketService.CreateTicket(request));
        //}
    }
}
