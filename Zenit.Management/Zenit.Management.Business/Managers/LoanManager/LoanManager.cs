using Zenit.Management.Data.Entities;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Management.Business.Managers.LoanManager
{
    public class LoanManager(IRepository<Loan> repository) : ManagementDomainService<Loan>(repository)
    {
    }
}