using Zenit.Scheduler.Common.Enums;

namespace Zenit.Scheduler.Contract.Requests
{
    public class UpdateJobRequest
    {
        public Guid Id { get; set; }
        public JobStatus? JobStatus { get; set; }
        public string? Data { get; set; }
        public Guid? ObjectId { get; set; }
    }

    public class UpdateJobResponse
    {
        public Guid Id { get; set; }
        public JobStatus? JobStatus { get; set; }
        public string? Data { get; set; }
        public Guid? ObjectId { get; set; }
    }
}