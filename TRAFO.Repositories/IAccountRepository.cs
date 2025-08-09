using TRAFO.Logic.Dto;

namespace TRAFO.Repositories;

public interface IAccountRepository
{
    // todo #84: this is just a dummy setup needed for implementation of other repositories. Make the actual thing    
    public void CreateIfNotExists(IEnumerable<Account> accountsToAdd);
}
