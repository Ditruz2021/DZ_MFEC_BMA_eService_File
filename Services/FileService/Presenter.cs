
using System.ComponentModel.DataAnnotations;

namespace dotnet_starter.Services.Images
{


    public class FileLinkRequest
    {
        [Required(ErrorMessage = "Bucket Name is required.")]
        public string Bucket { get; set; } = string.Empty;

        [Required(ErrorMessage = "File is required.")]
        public IFormFile File { get; set; } = null!;
    }


    public class FileUnlinkRequest
    {
        [Required(ErrorMessage = "Path Image is required.")]
        public string Path { get; set; } = string.Empty;
    }

    public class FileResponse
    {
        public string Path { get; set; } = string.Empty;
    }

}
