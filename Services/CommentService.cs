using Microsoft.EntityFrameworkCore;
using ticketManage.Data;
using ticketManage.DTOs.Comment;
using ticketManage.Exceptions;
using ticketManage.Mappers;
using ticketManage.Models;
using ticketManage.Utils;

namespace ticketManage.Services
{
    public class CommentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FileAssetService _fileAssetService;
        private readonly AppDbContext _context;
        private readonly TicketService _ticketService;
        private readonly UserService _userService;

        public CommentService(
            UserService userService,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext appDbContext,
            FileAssetService fileAssetService,
            TicketService ticketService
        )
        {
            _userService = userService;
            _fileAssetService = fileAssetService;
            _context = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            _ticketService = ticketService;
        }

        public async Task UpsertComment(UpsertCommentRequest upsertCommentRequest)
        {
            switch (upsertCommentRequest.GetOperation())
            {
                case Utils.ChangeOperation.Create:
                {
                    await this.CreateComment(upsertCommentRequest);
                    break;
                }
                case Utils.ChangeOperation.Update:
                {
                    await this.UpdateComment(upsertCommentRequest);
                    break;
                }
                case Utils.ChangeOperation.Delete:
                {
                    await this.SoftDeleteComment(upsertCommentRequest);
                    break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        private async Task SoftDeleteComment(UpsertCommentRequest upsertCommentRequest)
        {
            var comment = await GetCommentChecked(upsertCommentRequest.Id!.Value);
            comment.IsUpdated = true;
            comment.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<Comment?> GetComment(long id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment> GetCommentChecked(long id)
        {
            return await this.GetComment(id) ?? throw new NotFoundException("Not found");
        }

        private async Task UpdateComment(UpsertCommentRequest upsertCommentRequest)
        {
            var comment = await GetCommentChecked(upsertCommentRequest.Id!.Value);
            //validate
            comment.Content = upsertCommentRequest.Content;

            if (
                upsertCommentRequest.Attachments != null
                && upsertCommentRequest.Attachments.Length > 0
            )
            {
                FileAsset[] fileAttachments =
                    await _fileAssetService.AddLocalDatabaseFileAssetsAsync(
                        upsertCommentRequest.Attachments,
                        FileAssetPurpose.CommentAttachment
                    );

                foreach (var attachment in fileAttachments)
                {
                    comment.Attachments.Add(attachment);
                }
            }
            comment.IsUpdated = true;
            await _context.SaveChangesAsync();
        }

        public async Task CreateComment(UpsertCommentRequest upsertCommentRequest)
        {
            FileAsset[] fileAttachments = await _fileAssetService.AddLocalDatabaseFileAssetsAsync(
                upsertCommentRequest.Attachments ?? [],
                FileAssetPurpose.TicketAttachment
            );
            var ticket = await _ticketService.GetTicket(upsertCommentRequest.TicketId);
            //var parentComment=Utility.IsNotNullOrEmpty(upsertCommentRequest.ParentId) ?await GetComment(upsertCommentRequest.ParentId!.Value):null;
            var parentComment = upsertCommentRequest.ParentId is long parentId
                ? await GetComment(parentId)
                : null;
            //add validate Data,ticket later
            await _context.Comments.AddAsync(
                Comment.CreateNew(
                    await _userService.GetCurrentUser(),
                    ticket,
                    upsertCommentRequest.Content,
                    parentComment,
                    fileAttachments
                )
            );
            await _context.SaveChangesAsync();
        }

        public async Task<List<CommentsResponse>> GetCommentsResponseByTicketId(long ticketId)
        {
            return CommentMapper.ToCommentResponseList(
                await _context
                    .Comments.Where(comment => comment.TicketId == ticketId)
                    .Include(comment => comment.CreatedByUser)
                    .Include(comment => comment.Attachments)
                    .Include(comment => comment.Replies)
                    .ToListAsync()
            );
        }
    }
}
