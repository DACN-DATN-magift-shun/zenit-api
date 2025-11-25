using Microsoft.Extensions.DependencyInjection;

using Zenit.Share.Business;
using Zenit.Statistics.Common.Models;

namespace Zenit.Statistics.Business
{
    public class StatisticsApplicationService(IServiceProvider serviceProvider)
        : ApplicationServiceBase(serviceProvider)
    {
        public StatisticsCurrentAccount CurrentAccount => ServiceProvider.GetService<StatisticsCurrentAccount>();
    }
}