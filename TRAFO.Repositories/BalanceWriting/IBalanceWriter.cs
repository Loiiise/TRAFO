using TRAFO.Logic;

namespace TRAFO.Repositories.BalanceWriting;

// todo #77
public interface IBalanceWriter
{
    void WriteBalance(Balance balance);
}
