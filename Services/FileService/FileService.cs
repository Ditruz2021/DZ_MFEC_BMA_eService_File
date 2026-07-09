using dotnet_starter.Models;
using dotnet_starter.Presenters;
using dotnet_starter.Utils;

namespace dotnet_starter.Services.Images
{
    public interface IImageService
    {
        Task<Result<string>> LinkFile(FileLinkRequest request);
        Task<Result<string>> LinkFileResize(FileLinkRequest request);
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

        public async Task<Result<string>> LinkFile(FileLinkRequest request)
        {
            try
            {
                var uploadInfo = await Task.Run(() =>
                    _imageUploader.LinkFile(request.File, request.Bucket)
                );

                return Result<string>.Success(uploadInfo.Path);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("Upload failed: " + ex.Message);
            }
        }
        public async Task<Result<string>> LinkFileResize(FileLinkRequest request)
        {
            try
            {
                var uploadInfo = await Task.Run(() =>
                    _imageUploader.LinkFileResize(request.File, request.Bucket)
                );

                return Result<string>.Success(uploadInfo.Path);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("Upload failed: " + ex.Message);
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
