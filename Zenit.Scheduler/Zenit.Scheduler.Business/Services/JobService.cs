using Mapster;

using Zenit.Scheduler.Business.Managers;
using Zenit.Scheduler.Common.Enums;
using Zenit.Scheduler.Contract.Requests;
using Zenit.Scheduler.Data.Entities;

namespace Zenit.Scheduler.Business.Services
{
    public class JobService(IServiceProvider serviceProvider) : SchedulerApplicationService(serviceProvider)
    {
        private JobManager _JobManager => GetService<JobManager>();

        public Task<GetJobResponse> GetJob(GetJobRequest request)
        {
            var job = _JobManager.FindBy(job => job.Id == request.Id).FirstOrDefault();
            return Task.FromResult(Mapper.Map<GetJobResponse>(job!));
        }

        public async Task<CreateJobResponse> CreateJob(CreateJobRequest request)
        {
            var job = Mapper.Map<Job>(request);
            _JobManager.Add(job);
            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<CreateJobResponse>(job);
        }

        public async Task<UpdateJobResponse> UpdateJob(UpdateJobRequest request)
        {
            var job = _JobManager.FindBy(job => job.Id == request.Id).FirstOrDefault();
            request.Adapt(job);
            _JobManager.Update(job!);
            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<UpdateJobResponse>(job);
        }

        public async Task DeleteJob(DeleteJobRequest request)
        {
            var job = _JobManager.FindBy(job => job.Id == request.Id).FirstOrDefault();
            job.JobStatus = JobStatus.Deleted;
            _JobManager.Delete(job);
            await UnitOfWork.SaveChangesAsync();
        }
    }
}