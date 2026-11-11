using System.Net.Mail;
using ticketManage.Utils;

namespace ticketManage.Models
{
    public class Comment : AuditableEntity
    {
        public long Id { get; set; }
        public long TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;
        public string CreatedByUserId { get; set; } = null!;
        public User CreatedByUser { get; set; } = null!;
        public long? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = [];
        public ICollection<FileAsset> Attachments { get; set; } = [];
        public bool IsDeleted { get; set; }
        public string Content { get; set; } = string.Empty;

        public bool IsUpdated { get; set; } = false;

        public static Comment CreateNew(
            User user,
            Ticket ticket,
            string content,
            Comment? parentComment,
            ICollection<FileAsset> Attachments
        )
        {
            var comment = new Comment
            {
                Ticket = ticket,
                TicketId = ticket.Id,
                CreatedByUser = user,
                CreatedByUserId = user.Id,
            };
            if (Utility.IsNotNullOrEmpty(parentComment))
            {
                comment.ParentComment = parentComment;
                comment.ParentCommentId = parentComment!.Id;
            }
            if (Utility.IsNotNullOrEmpty(Attachments))
            {
                comment.Attachments = Attachments;
            }
            if (Utility.IsNotNullOrEmpty(content))
            {
                comment.Content = content;
            }
            return comment;
        }
    }
}
