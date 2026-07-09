using dotnet_starter.Models;
using dotnet_starter.Presenters;
using dotnet_starter.Utils;

namespace dotnet_starter.Services.Images
{
    public interface IImageService
    {
        Task<FileResponse?> LinkFile(FileLinkRequest request);
        Task<Result<FileResponse>> LinkFileResize(FileLinkRequest request);
        Task<Result<string>> UnlinkFile(FileUnlinkRequest request);
    }

    public class ImageService : IImageService
    {
        private readonly IImageUploader _imageUploader;

        public ImageService
        (
            IImageUploader imageUploader
        )
        {
            _imageUploader = imageUploader;
        }

        public async Task<FileResponse?> LinkFile(FileLinkRequest request)
        {
            try
            {
                var uploadInfo = await Task.Run(() =>
                    _imageUploader.LinkFile(request.File, request.Bucket, request.FileName)
                );



                return new FileResponse
                {
                    Path = uploadInfo.Path,
                    Bucket = request.Bucket,
                    FileType = uploadInfo.FileType,
                    FileSize = uploadInfo.FileSize
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<Result<FileResponse>> LinkFileResize(FileLinkRequest request)
        {
            try
            {
                var uploadInfo = await Task.Run(() =>
                    _imageUploader.LinkFileResize(request.File, request.Bucket, request.FileName)
                );

                return Result<FileResponse>.Success(new FileResponse
                {
                    Path = uploadInfo.Path,
                    Bucket = request.Bucket,
                    FileType = uploadInfo.FileType,
                    FileSize = uploadInfo.FileSize
                });
            }
            catch (Exception ex)
            {
                return Result<FileResponse>.Fail("Upload failed: " + ex.Message);
            }
        }


        public async Task<Result<string>> UnlinkFile(FileUnlinkRequest request)
        {
            try
            {
                var msg = await Task.Run(() =>
                    _imageUploader.UnlinkFile(request.Path)
                );

                return Result<string>.Success(msg);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(ex.Message);
            }
        }
    }
}
