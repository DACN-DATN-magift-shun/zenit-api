using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

using Mapster;

using Zenit.Management.Business.Managers.PhotoManager;
using Zenit.Management.Contract.Requests.PhotoRequests;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Constants;
using Zenit.Share.Contract.Models;

namespace Zenit.Management.Business.Services.PhotoServices
{
    public class PhotoService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        private PhotoManager _PhotoManager => GetService<PhotoManager>();
        private string _S3UploadUri => GetS3UploadUri();

        private string GetS3UploadUri()
        {
            return Environment.GetEnvironmentVariable(EnvConstants.S3_UPLOAD_URI) ?? throw new Exception($"Environment variable '{EnvConstants.S3_UPLOAD_URI}' is not set.");
        }

        public async Task<CreatePhotoResponse> Create(CreatePhotoRequest request)
        {
            var file = request.File;
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File empty.");
            }

            var ext = Path.GetExtension(file.FileName);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!allowedExtensions.Contains(ext.ToLower()))
            {
                throw new ArgumentException("Invalid file extension.");
            }

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = $"{_S3UploadUri}{fileName}";

            // Upload file to S3
            var s3Client = new AmazonS3Client(RegionEndpoint.APSoutheast1);
            var s3Bucket = Environment.GetEnvironmentVariable(EnvConstants.AWS_S3_BUCKET) ?? throw new Exception($"Environment variable '{EnvConstants.AWS_S3_BUCKET}' is not set.");

            using (var stream = file.OpenReadStream())
            {
                await s3Client.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = s3Bucket,
                    Key = $"uploads/{fileName}",
                    InputStream = stream,
                    ContentType = file.ContentType
                });
            }

            var photo = _PhotoManager.Add(new Photo
            {
                Id = Guid.NewGuid(),
                FileName = fileName,
                FilePath = filePath,
                Size = file.Length,
                ContentType = file.ContentType,
                TransactionId = request.TransactionId ?? null
            });
            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<CreatePhotoResponse>(photo);
        }

        public async Task Delete(DeletePhotoRequest request)
        {
            var photo = _PhotoManager.FindBy(p => p.Id == request.Id && p.IsDeleted == false).FirstOrDefault();
            if (photo == null)
            {
                throw new Exception("Photo not found");
            }

            _PhotoManager.Delete(photo);
            await UnitOfWork.SaveChangesAsync();
        }

        public Task<GetAllPhotoResponse> GetAll(GetAllPhotoRequest request)
        {
            if (request.TransactionId.HasValue)
            {
                var photos = _PhotoManager.FindBy(p => p.TransactionId == request.TransactionId && p.IsDeleted == false);
                return Task.FromResult(Mapper.Map<GetAllPhotoResponse>(
                    PaginationResponse<Photo>.Create(photos, request)
                ));
            }
            else
            {
                var photo = _PhotoManager.FindBy(p => p.IsDeleted == false && p.TransactionId == null);
                return Task.FromResult(Mapper.Map<GetAllPhotoResponse>(
                    PaginationResponse<Photo>.Create(photo, request)
                ));
            }
        }

        public Task<GetDetailPhotoResponse> GetDetail(GetDetailPhotoRequest request)
        {
            var photo = _PhotoManager.FindBy(p => p.Id == request.Id && p.IsDeleted == false).FirstOrDefault();
            if (photo == null)
            {
                throw new Exception("Photo not found");
            }
            return Task.FromResult(Mapper.Map<GetDetailPhotoResponse>(photo));
        }
    }
}