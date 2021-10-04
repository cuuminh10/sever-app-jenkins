using gmc_api.Base.dto;
using gmc_api.Base.InterFace;
using gmc_api.DTO.FC;
using gmc_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Controllers
{

    [ApiController()]
    [Route("fc")]
    public class FileCommentController : ControllerBase
    {
        private readonly IStorageService _storage;
        private readonly IADAttachmentService _attach;
        private readonly IADCommentService _comment;

        public FileCommentController(IStorageService storage, IADAttachmentService attach, IADCommentService comment)
        {
            _storage = storage;
            _attach = attach;
            _comment = comment;
        }

        [HttpPost("fileUpload/{type}/{objectId}")]
        public async Task<IActionResult> UploadDocument(
        string type, int objectId, [FromForm(Name = "file")] IFormFile file)
        {
            if (!FileUploadType.isIn(type))
            {
                return BadRequest(new { message = FileUploadType.messageValidate() });
            }
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            var pathFile = _storage.Upload(type, file);
            ADAttachmentCreateRequest attchObj = new();
            if (objectId > 0)
            {
                attchObj.ADAttachmentRefID = objectId.ToString();
                attchObj.ADAttachmentTable = FileUploadType.UploadTable[type];
            }
            else
            {
                attchObj.ADAttachmentRefID = "0";
            }

            attchObj.ADAttachmentName = file.FileName;
            attchObj.ADAttachmentPath = pathFile;

            var rs = _attach.CreateObject(attchObj, userInfo);
            return await Task.FromResult(Ok(rs));
        }

        [HttpPost("comment/{type}/{objectId}")]
        public async Task<IActionResult> commentAdd(
        string type, int objectId, ADCommentCreateRequest content)
        {
            if (!FileUploadType.isIn(type))
            {
                return BadRequest(new { message = FileUploadType.messageValidate() });
            }
            UserLoginInfo userInfo = (UserLoginInfo)HttpContext.Items["User"];
            if (objectId > 0)
            {
                content.ADCommentRefID = objectId.ToString();
                content.ADCommentTable = FileUploadType.UploadTable[type];
            }
            else
            {
                content.ADCommentRefID = "0";
            }


            var rs = _comment.CreateObject(content, userInfo);
            return await Task.FromResult(Ok(_comment.ConvertComment(rs)));
        }

        [HttpGet("viewFile/{type}/{fileName}")]
        public async Task<IActionResult> ViewDocument(string type, string fileName)
        {
            if (!FileUploadType.isIn(type))
            {
                return BadRequest(new { message = FileUploadType.messageValidate() });
            }
            var pathFile = _storage.GetPathByName(type, fileName);
            return File(await System.IO.File.ReadAllBytesAsync(pathFile), "application/octet-stream", fileName);
        }
    }
}
