using MediatR;

using Zenit.Share.Data;

namespace Zenit.Management.Data
{
    public class ManagementUnitOfWork(
        ManagementDbContext context, IPublisher publisher) : UnitOfWorkBase<ManagementDbContext>(context, publisher)
    {
    }
}