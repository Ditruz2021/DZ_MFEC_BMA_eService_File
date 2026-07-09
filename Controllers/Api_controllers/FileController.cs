

using dotnet_starter.Services.Images;
using dotnet_starter.Presenters;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_starter.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class ImageApiController : ControllerBase
    {
        private readonly IImageService _ImageService;

        public ImageApiController(IImageService ImageService)
        {
            _ImageService = ImageService;
        }

        [HttpPost("file-link")]
        public async Task<IActionResult> LinkFile([FromForm] FileLinkRequest request)
        {
            var res = await _ImageService.LinkFile(request);
            if (!res.IsSuccess || res.Data == null)
                return BadRequest(ResponseError<object>.Error(res.Error ?? "Upload failed", HttpContext));

            return Ok(ResponseSuccess<string>.Success(res.Data, HttpContext));
        }


        [HttpPost("file-link-resize")]
        public async Task<IActionResult> LinkFileResize([FromForm] FileLinkRequest request)
        {
            var res = await _ImageService.LinkFileResize(request);

            if (!res.IsSuccess || res.Data == null)
                return BadRequest(ResponseError<string>.Error(res.Error ?? "Unknown error", HttpContext));

            return Ok(ResponseSuccess<string>.Success(res.Data, HttpContext));
        }

        [HttpPost("file-unlink")]
        public async Task<IActionResult> UnlinkFile([FromBody] FileUnlinkRequest request)
        {
            var res = await _ImageService.UnlinkFile(request);
            if (!res.IsSuccess)
                return NotFound(ResponseError<object>.Error(res.Error!, HttpContext));

            return Ok(ResponseSuccess<string>.Success(res.Data!, HttpContext));
        }
    }
}

