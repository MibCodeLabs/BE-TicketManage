using ticketManage.DTOs.FileAsset;
using ticketManage.DTOs.User;

namespace ticketManage.DTOs.Comment
{
    public class CommentsResponse
    {
        public BasicUserWithDp User { get; set; } = null!;
        public long? ParentId { get; set; }
        public long? TicketId { get; set; }
        public string Content { get; set; } = "";
        public List<BasicFileAsset>? Attachments { get; set; } = [];
        public CommentsResponse[] Replies { get; set; } = [];
    };
}
