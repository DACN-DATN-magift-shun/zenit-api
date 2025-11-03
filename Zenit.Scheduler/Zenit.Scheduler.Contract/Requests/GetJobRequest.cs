using Zenit.Scheduler.Common.Enums;

namespace Zenit.Scheduler.Contract.Requests
{
    public class GetJobRequest
    {
        public Guid Id { get; set; }
    }

    public class GetJobResponse
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Guid JobId { get; set; }
        public JobEvent? JobEvent { get; set; }
        public JobType? JobType { get; set; }
        public JobStatus? JobStatus { get; set; }
        public string? Data { get; set; }
        public Guid? ObjectId { get; set; }
        public DateTime? SendTime { get; set; }
    }
}