using Microsoft.AspNetCore.Mvc;
using ticketManage.DTOs.Comment;
using ticketManage.Services;

namespace ticketManage.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private CommentService _commentService;
        private ILogger<CommentController> _logger;

        public CommentController(CommentService commentService, ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UpsertComment(UpsertCommentRequest upsertCommentRequest)
        {
            try
            {
                await _commentService.UpsertComment(upsertCommentRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet("ticket/{id}")]
        public async Task<IActionResult> GetTicketComments([FromRoute(Name = "id")] long ticketId)
        {
            try
            {
                return Ok(await _commentService.GetCommentsResponseByTicketId(ticketId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
    }
}
