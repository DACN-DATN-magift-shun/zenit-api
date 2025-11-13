using MapsterMapper;

using Microsoft.Extensions.DependencyInjection;

using Zenit.Share.Business.Interfaces;
using Zenit.Share.Data.Interfaces;


namespace Zenit.Share.Business
{
    public abstract class ApplicationServiceBase(IServiceProvider serviceProvider) : IApplicationService
    {
        protected IServiceProvider ServiceProvider { get; } = serviceProvider;
        protected IMapper Mapper => ServiceProvider.GetRequiredService<IMapper>();
        protected IUnitOfWork UnitOfWork => ServiceProvider.GetRequiredService<IUnitOfWork>();

        public TService GetService<TService>() where TService : class
        {
            return ServiceProvider.GetRequiredService<TService>();
        }
    }
}
