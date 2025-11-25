using MediatR;

using Zenit.Share.Data;

namespace Zenit.Management.Data
{
    public class ManagementUnitOfWork(
        ManagementDbContext context) : UnitOfWorkBase<ManagementDbContext>(context)
    {
    }
}