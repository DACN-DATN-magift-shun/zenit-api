using MediatR;

using Microsoft.AspNetCore.Http;

namespace Zenit.Management.Contract.Requests.PhotoRequests
{
    public class CreatePhotoRequest : IRequest<CreatePhotoResponse>
    {
        public required IFormFile File { get; set; }
        public Guid? TransactionId { get; set; }
    }

    public class CreatePhotoResponse
    {
        public required Guid Id { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public decimal? Size { get; set; }
        public string? ContentType { get; set; }
        public Guid? TransactionId { get; set; }
    }
}