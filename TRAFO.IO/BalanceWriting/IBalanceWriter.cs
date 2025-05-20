using TRAFO.Logic;

namespace TRAFO.IO.BalanceWriting;

public interface IBalanceWriter
{
    void WriteBalance(Balance balance);
}
