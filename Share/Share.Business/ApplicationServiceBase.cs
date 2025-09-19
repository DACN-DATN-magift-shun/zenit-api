using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

using Share.Business.Interfaces;
using Share.Data.Interfaces;


namespace Share.Business
{
    public abstract class ApplicationServiceBase(IServiceProvider serviceProvider) : IApplicationService
    {
        private IServiceProvider ServiceProvider { get; } = serviceProvider;
        protected IMapper Mapper => ServiceProvider.GetRequiredService<IMapper>();
        protected IUnitOfWork UnitOfWork => ServiceProvider.GetRequiredService<IUnitOfWork>();

        public TService GetService<TService>() where TService : class
        {
            return ServiceProvider.GetRequiredService<TService>();
        }
    }
}