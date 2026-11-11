using Microsoft.EntityFrameworkCore;
using ticketManage.Data;
using ticketManage.DTOs.FileAsset;
using ticketManage.DTOs.Ticket;
using ticketManage.DTOs.User;
using ticketManage.Exceptions;
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

        public async Task<Ticket> GetTicket(long id)
        {
            return await _context.Tickets.FindAsync(id)
                ?? throw new InvalidDataException("Not found");
        }

        public async Task<TicketResponse> UpsertTicket(UpsertTicketRequest request)
        {
            Ticket ticket;
            bool isNew = !Utility.IsNotNullOrEmpty(request.Id);

            if (!isNew)
            {
                ticket =
                    await _context
                        .Tickets.Include(t => t.IssuedByUser)
                        .Include(t => t.AssignedToUser)
                        .Include(t => t.Attachments)
                        .FirstOrDefaultAsync(t => t.Id == request.Id.Value)
                    ?? throw new NotFoundException("Ticket Not found");

                ticket.Title = request.Title;
                ticket.Expiry = request.ExpiryDate;
                ticket.Status = request.Status;
                ticket.Content = request.Content;

                if (Utility.IsNotNullOrEmpty(request?.AssignedTo?.Id))
                {
                    ticket.AssignedToUser = await _userService.GetUserById(request.AssignedTo.Id);
                }

                if (request.Attachments != null && request.Attachments.Length > 0)
                {
                    FileAsset[] fileAttachments =
                        await _fileAssetService.AddLocalDatabaseFileAssetsAsync(
                            request.Attachments,
                            FileAssetPurpose.TicketAttachment
                        );

                    foreach (var attachment in fileAttachments)
                    {
                        ticket.Attachments.Add(attachment);
                    }
                }
            }
            else
            {
                FileAsset[] fileAttachments =
                    await _fileAssetService.AddLocalDatabaseFileAssetsAsync(
                        request.Attachments ?? [],
                        FileAssetPurpose.TicketAttachment
                    );

                var assignedUser =
                    (
                        Utility.IsNotNullOrEmpty(request.AssignedTo)
                        && Utility.IsNotNullOrEmpty(request.AssignedTo!.Id)
                    )
                        ? await _userService.GetUserById(request.AssignedTo.Id)
                        : null;

                ticket = Ticket.CreateNew(
                    request.Title,
                    request.Content,
                    await _userService.GetCurrentUser(),
                    assignedUser,
                    request.ExpiryDate,
                    request.Status,
                    fileAttachments
                );

                _context.Tickets.Add(ticket);
            }

            await _context.SaveChangesAsync();
            return TicketMapper.toTicketResponse(ticket);
        }

        public IQueryable<TicketResponse> GetTickets()
        {
            return _context
                .Tickets.AsNoTracking()
                .Select(t => new TicketResponse
                {
                    Id = t.Id,
                    Status = t.Status,
                    IssuedByUser = BasicUser.Create(t.IssuedByUser),
                    ExpiryDate = t.Expiry,
                    Content = t.Content,
                    Title = t.Title,
                    AssignedTo = Utility.IsNotNullOrEmpty(t.AssignedToUser)
                        ? BasicUser.Create(t.AssignedToUser!)
                        : null,
                    Attachments = BasicFileAsset.ConvertToList(t.Attachments!.ToList()),
                });
        }
    }
}
