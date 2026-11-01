using Microsoft.AspNetCore.Mvc;
using ticketManage.Services;

namespace ticketManage.Controller
{
    [Route("res")]
    [ApiController]
    public class FileAssetController : ControllerBase
    {
        private FileAssetService _fileAssetService;

        public FileAssetController(FileAssetService fileAssetService)
        {
            _fileAssetService = fileAssetService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile([FromRoute] long id)
        {
            var fullFile = await _fileAssetService.GetFile(id);

            return File(fullFile.Data, fullFile.FileAsset.ContentType, fullFile.FileAsset.FileName);
        }
    }
}
