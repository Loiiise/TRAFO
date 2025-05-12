using TRAFO.Logic;

namespace TRAFO.IO.BalanceReading;
public interface IBalanceReader
{
    Balance ReadBalance(string identifier);
}
