using TRAFO.Logic.Dto;

namespace TRAFO.Repositories.BalanceWriting;

// todo #77
public interface IBalanceWriter
{
    void WriteBalance(Balance balance);
}
