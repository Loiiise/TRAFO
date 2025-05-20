using TRAFO.Logic;

namespace TRAFO.IO.BalanceReading;
public interface IBalanceReader
{
    IEnumerable<Balance> ReadBalances(string identifier);
}
