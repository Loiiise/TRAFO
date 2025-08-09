using TRAFO.Logic.Dto;
using TRAFO.Repositories.Entities;

namespace TRAFO.Repositories;

public class AccountRepository : IAccountRepository
{
    public void CreateIfNotExists(IEnumerable<Account> accountsToAdd)
    {
        throw new NotImplementedException();
    }

    internal static Account FromDatabaseEntry(AccountBalanceDatabaseEntry account)
    {
        // todo #84
        throw new NotImplementedException();
    }
}
