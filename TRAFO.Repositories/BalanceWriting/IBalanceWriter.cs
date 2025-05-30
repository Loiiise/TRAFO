using TRAFO.Logic;

namespace TRAFO.Repositories.BalanceWriting;

public interface IBalanceWriter
{
    void WriteBalance(Balance balance);
}
