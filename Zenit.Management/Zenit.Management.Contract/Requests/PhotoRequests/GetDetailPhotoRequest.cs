using MediatR;

namespace Zenit.Management.Contract.Requests.PhotoRequests
{
    public class GetDetailPhotoRequest : IRequest<GetDetailPhotoResponse>
    {
        public required Guid Id { get; set; }
    }

    public class GetDetailPhotoResponse
    {
        public required Guid Id { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public decimal? Size { get; set; }
        public string? ContentType { get; set; }
        public Guid? TransactionId { get; set; }
    }
}