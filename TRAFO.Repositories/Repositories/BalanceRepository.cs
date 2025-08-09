using TRAFO.Logic.Dto;
using TRAFO.Repositories.Entities;

namespace TRAFO.Repositories.Repositories;
public sealed class BalanceRepository : IBalanceRepository
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

    internal static Balance FromDatabaseEntry(BalanceDatabaseEntry balance)
    {
        // todo #85
        throw new NotImplementedException();
        /*
        return new Balance
        {
            Amount = balance.Amount,
            Currency = balance.Currency,
            ThisPartyIdentifier = balance.ThisPartyIdentifier,
            Timestamp = balance.Timestamp,
        };
        */
    }

    internal BalanceDatabaseEntry ToDatabaseEntry(Balance balance)
    {
        // todo #85
        throw new NotImplementedException();
        /*

        return new BalanceDatabaseEntry
        {
            Amount = balance.Amount,
            Currency = balance.Currency,
            ThisPartyIdentifier = balance.ThisPartyIdentifier,
            Timestamp = balance.Timestamp,
        };
        */
    }
}
