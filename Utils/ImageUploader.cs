using dotnet_starter.Services.Images;
using ImageMagick;

namespace dotnet_starter.Utils
{
    public interface IImageUploader
    {
        FileResponse LinkFile(IFormFile file, string bucket);
        FileResponse LinkFileResize(IFormFile file, string bucket);
        string UnlinkFile(string path);
    }

    public class ImageUploader : IImageUploader
    {
        private readonly string _rootPath;


        public ImageUploader()
        {
            _rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/storage");
        }

        public FileResponse LinkFile(IFormFile file, string bucket)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null.", nameof(file));

            if (string.IsNullOrWhiteSpace(bucket))
                throw new ArgumentException("Folder name cannot be empty.", nameof(bucket));

            string uploadPath = Path.Combine(_rootPath, bucket);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string fileExtension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(fileExtension))
                throw new InvalidOperationException("Invalid file format. Missing extension.");

            string sanitizedFileName = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}{fileExtension}";
            string filePath = Path.Combine(uploadPath, sanitizedFileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return new FileResponse
                {
                    Path = $"/storage/{bucket}/{sanitizedFileName}"
                };
            }
            catch (IOException ioEx)
            {
                throw new IOException("❌ An error occurred while writing the file to disk.", ioEx);
            }
            catch (Exception ex)
            {
                throw new IOException("❌ Unexpected error while uploading image.", ex);
            }
        }
        public FileResponse LinkFileResize(IFormFile file, string bucket)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null.", nameof(file));

            if (string.IsNullOrWhiteSpace(bucket))
                throw new ArgumentException("Folder name cannot be empty.", nameof(bucket));

            string uploadPath = Path.Combine(_rootPath, bucket);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (string.IsNullOrEmpty(fileExtension))
                throw new InvalidOperationException("Invalid file format. Missing extension.");

            string sanitizedFileName = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}{fileExtension}";
            string filePath = Path.Combine(uploadPath, sanitizedFileName);

            try
            {
                var isImage = new[] { ".jpg", ".jpeg", ".png", ".webp", ".avif", ".gif" }.Contains(fileExtension);

                if (isImage)
                {
                    using var inputStream = file.OpenReadStream();
                    using var image = new MagickImage(inputStream);

                    if (image.Width > 720)
                    {
                        image.Resize(new MagickGeometry(720) { IgnoreAspectRatio = false });
                    }

                    image.AutoOrient();
                    image.Format = fileExtension switch
                    {
                        ".png" => MagickFormat.Png,
                        ".webp" => MagickFormat.WebP,
                        ".avif" => MagickFormat.Avif,
                        ".gif" => MagickFormat.Gif,
                        _ => MagickFormat.Jpeg
                    };

                    image.Write(filePath);
                }
                else
                {
                    using var stream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(stream);
                }

                return new FileResponse
                {
                    Path = $"/storage/{bucket}/{sanitizedFileName}"
                };
            }
            catch (Exception ex)
            {
                throw new IOException("❌ Error while uploading or resizing image.", ex);
            }
        }
        // public FileResponse LinkFile(IFormFile file, string bucket, string fileName)
        // {
        //     if (file == null || file.Length == 0)
        //         throw new ArgumentException("File is empty or null.", nameof(file));

        //     if (string.IsNullOrWhiteSpace(bucket))
        //         throw new ArgumentException("Folder name cannot be empty.", nameof(bucket));

        //     string uploadPath = Path.Combine(_rootPath, bucket);
        //     if (!Directory.Exists(uploadPath))
        //     {
        //         Directory.CreateDirectory(uploadPath);
        //     }

        //     string fileExtension = Path.GetExtension(file.FileName);
        //     if (string.IsNullOrEmpty(fileExtension))
        //         throw new InvalidOperationException("Invalid file format. Missing extension.");

        //     string newFileName = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}{fileExtension}";
        //     string filePath = Path.Combine(uploadPath, newFileName);

        //     try
        //     {
        //         using (var stream = new FileStream(filePath, FileMode.Create))
        //         {
        //             file.CopyTo(stream);
        //         }
        //         var requestContext = _httpContextAccessor.HttpContext?.Request;
        //         var baseUrl = $"{requestContext?.Scheme}://{requestContext?.Host}";
        //         return new FileResponse
        //         {
        //             Path = baseUrl + $"/storage/{bucket}/{newFileName}",
        //             FileType = file.ContentType,
        //             FileSize = Math.Round((file.Length / 1024f), 2).ToString() + " KB"
        //         };
        //     }
        //     catch (IOException ioEx)
        //     {
        //         throw new IOException("❌ An error occurred while writing the file to disk.", ioEx);
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new IOException("❌ Unexpected error while uploading image.", ex);
        //     }
        // }
        // public FileResponse LinkFileResize(IFormFile file, string bucket, string fileName)
        // {
        //     if (file == null || file.Length == 0)
        //         throw new ArgumentException("File is empty or null.", nameof(file));

        //     if (string.IsNullOrWhiteSpace(bucket))
        //         throw new ArgumentException("Folder name cannot be empty.", nameof(bucket));

        //     string uploadPath = Path.Combine(_rootPath, bucket);
        //     if (!Directory.Exists(uploadPath))
        //     {
        //         Directory.CreateDirectory(uploadPath);
        //     }

        //     string fileExtension = Path.GetExtension(file.FileName).ToLower();
        //     if (string.IsNullOrEmpty(fileExtension))
        //         throw new InvalidOperationException("Invalid file format. Missing extension.");

        //     string newFileName = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}{fileExtension}";
        //     string filePath = Path.Combine(uploadPath, newFileName);

        //     try
        //     {
        //         var isImage = new[] { ".jpg", ".jpeg", ".png", ".webp", ".avif", ".gif" }.Contains(fileExtension);

        //         if (isImage)
        //         {
        //             using var inputStream = file.OpenReadStream();
        //             using var image = new MagickImage(inputStream);

        //             if (image.Width > 720)
        //             {
        //                 image.Resize(new MagickGeometry(720) { IgnoreAspectRatio = false });
        //             }

        //             image.AutoOrient();
        //             image.Format = fileExtension switch
        //             {
        //                 ".png" => MagickFormat.Png,
        //                 ".webp" => MagickFormat.WebP,
        //                 ".avif" => MagickFormat.Avif,
        //                 ".gif" => MagickFormat.Gif,
        //                 _ => MagickFormat.Jpeg
        //             };

        //             image.Write(filePath);
        //         }
        //         else
        //         {
        //             using var stream = new FileStream(filePath, FileMode.Create);
        //             file.CopyTo(stream);
        //         }

        //         var requestContext = _httpContextAccessor.HttpContext?.Request;
        //         var baseUrl = $"{requestContext?.Scheme}://{requestContext?.Host}";

        //         return new FileResponse
        //         {
        //             Path = baseUrl + $"/storage/{bucket}/{newFileName}",
        //             FileType = file.ContentType,
        //             FileSize = Math.Round((new FileInfo(filePath).Length / 1024f), 2).ToString() + " KB"
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new IOException("❌ Error while uploading or resizing image.", ex);
        //     }
        // }



        public string UnlinkFile(string path)
        {
            try
            {
                var uri = new Uri(path, UriKind.RelativeOrAbsolute);
                string relativePath = uri.IsAbsoluteUri ? uri.AbsolutePath.TrimStart('/') : path.TrimStart('/');

                if (relativePath.StartsWith("storage/"))
                {
                    relativePath = relativePath.Substring("storage/".Length);
                }

                string fullPath = Path.Combine(_rootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return "✅ Image deleted successfully.";
                }
                else
                {
                    return $"⚠️ Image not found at: {path}";
                }
            }
            catch (Exception ex)
            {
                throw new IOException("❌ Error while deleting image.", ex);
            }
        }

    }
}
