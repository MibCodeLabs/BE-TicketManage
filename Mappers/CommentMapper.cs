using Riok.Mapperly.Abstractions;
using ticketManage.DTOs.Comment;
using ticketManage.Models;

namespace ticketManage.Mappers
{
    [Mapper]
    public partial class CommentMapper
    {
        [MapProperty(nameof(Comment.ParentCommentId), nameof(CommentsResponse.ParentId))]
        public static partial CommentsResponse ToCommentResponse(Comment comment);

        public static partial List<CommentsResponse> ToCommentResponseList(List<Comment> comments);
    }
}
