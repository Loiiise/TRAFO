using TRAFO.Logic.Dto;

namespace TRAFO.Repositories;

internal interface IBalanceRepository :
    IBalanceReader,
    IBalanceWriter
{ }

public interface IBalanceReader
{
    IEnumerable<Balance> ReadBalances(string identifier);
}

// todo #85
public interface IBalanceWriter
{
    void WriteBalance(Balance balance);
}
