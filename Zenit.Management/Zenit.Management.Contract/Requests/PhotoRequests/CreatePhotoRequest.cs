using MediatR;

namespace Zenit.Management.Contract.Requests.PhotoRequests
{
    public class CreatePhotoRequest : IRequest<CreatePhotoResponse>
    {
        public string? FileName { get; set; }
        public string? RelativePath { get; set; }
        public decimal? Size { get; set; }
        public string? ContentType { get; set; }
        public Guid? TransactionId { get; set; }
    }

    public class CreatePhotoResponse
    {
        public required Guid Id { get; set; }
        public string? FileName { get; set; }
        public string? RelativePath { get; set; }
        public decimal? Size { get; set; }
        public string? ContentType { get; set; }
        public Guid? TransactionId { get; set; }
    }
}