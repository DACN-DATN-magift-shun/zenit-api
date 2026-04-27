using Microsoft.Extensions.DependencyInjection;

using Zenit.Management.Common.Models;
using Zenit.Share.Business;

namespace Zenit.Management.Business
{
    public class ManagementApplicationService(IServiceProvider serviceProvider) : ApplicationServiceBase(serviceProvider)
    {
        public ManagementCurrentAccount CurrentAccount => serviceProvider.GetService<ManagementCurrentAccount>();
    }
}