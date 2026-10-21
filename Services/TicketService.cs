using System.Net.Sockets;
using ticketManage.Data;
using ticketManage.DTOs.Shared;
using ticketManage.DTOs.Ticket;
using ticketManage.Mappers;
using ticketManage.Models;
using ticketManage.Utils;

namespace ticketManage.Services
{
    public class TicketService
    {
        private readonly AppDbContext _context;
        private readonly UserService _userService;
        private readonly FileAssetService _fileAssetService;

        public TicketService(
            AppDbContext context,
            UserService userService,
            FileAssetService fileAssetService
        )
        {
            _context = context;
            _userService = userService;
            _fileAssetService = fileAssetService;
        }

        public async Task<TicketResponse> GetTicket(long id)
        {
            var ticket =
                await _context.Tickets.FindAsync(id) ?? throw new InvalidDataException("Not found");
            return TicketMapper.toTicketResponse(ticket);
        }

        public async Task<TicketResponse> CreateTicket(CreateTicketRequest request)
        {
            //add transaction boudnary later and also check other places for same
            FileAsset[] fileAttachments = await _fileAssetService.AddLocalDatabaseFileAssetsAsync(
                request.Attachments ?? [],
                FileAssetPurpose.TicketAttachment
            );
            var ticket = Ticket.CreateNew(
                request.Title,
                request.Description,
                await _userService.GetCurrentUser(),
                (
                    Utility.IsNotNullOrEmpty(request.AssignedTo)
                    && Utility.IsNotNullOrEmpty(request.AssignedTo!.Id)
                )
                    ? await _userService.GetUserById(request.AssignedTo.Id)
                    : null,
                request.ExpiryDate,
                request.Status,
                fileAttachments
            );
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return TicketMapper.toTicketResponse(ticket);
        }
    }

    //public async Task<Page<TicketResponse>> getTickets(TicketPageRequest ticketPageRequest)
    //    {

    //        return
    //    }
}
