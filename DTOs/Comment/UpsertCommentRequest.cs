using ticketManage.DTOs.Shared;
using ticketManage.Utils;

namespace ticketManage.DTOs.Comment
{
    public class UpsertCommentRequest : ChangeOperationRequest<long?>
    {
        public long TicketId { get; set; }
        public long? ParentId { get; set; }
        public string Content { get; set; } = "";
        public IFormFile[]? Attachments { get; set; } = [];

        public CommentType GetCommentType()
        {
            if (Utility.IsNotNullOrEmpty(this.ParentId) && Utility.IsNotNullOrEmpty(this.TicketId))
            {
                throw new InvalidDataException("Invaild State cannot have both Ids");
            }
            if (Utility.IsNotNullOrEmpty(this.ParentId))
            {
                return CommentType.Reply;
            }
            else if (Utility.IsNotNullOrEmpty(this.TicketId))
            {
                return CommentType.Comment;
            }
            else
            {
                throw new InvalidDataException("Invaild State");
            }
        }
    };
}


//public long Id { get; set; }
//public long TicketId { get; set; }
//public Ticket Ticket { get; set; } = null!;
//public long? ParentCommentId { get; set; }
//public Comment? ParentComment { get; set; }
//public ICollection<Comment> Replies { get; set; } = [];
//public ICollection<FileAsset> Attachments { get; set; } = [];
//public bool IsDeleted { get; set; }
