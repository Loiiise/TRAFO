using TRAFO.Logic.Dto;

namespace TRAFO.Repositories.BalanceReading;
public interface IBalanceReader
{
    IEnumerable<Balance> ReadBalances(string identifier);
}
