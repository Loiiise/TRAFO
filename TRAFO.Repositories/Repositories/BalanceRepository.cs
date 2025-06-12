using TRAFO.Logic.Dto;

namespace TRAFO.Repositories.Repositories;
public sealed class BalanceRepository : EntityFrameworkDatabase, IBalanceRepository
{
    public IEnumerable<Balance> ReadBalances(string identifier)
    {
        // todo #85
        throw new NotImplementedException();
        //return _context.Balances.Where(b => b. == identifier).Select(FromDatabaseEntry);
    }

    public void WriteBalance(Balance balance)
    {
        // todo #85
        throw new NotImplementedException();
        /*
        _context.Balance.Add(ToDatabaseEntry(balance));
        _context.SaveChanges();
        */
    }
}
