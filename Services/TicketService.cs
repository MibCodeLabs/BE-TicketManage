using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ticketManage.Data;
using ticketManage.DTOs.Ticket;
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

        public async Task<Ticket> CreateTicket(CreateTicketRequest request)
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
                !string.IsNullOrEmpty(request.AssignedTo)
                    ? await _userService.GetUserById(request.AssignedTo)
                    : null,
                request.ExpiryDate,
                request.Status,
                fileAttachments
            );
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
    }
}
