using TRAFO.Logic.Dto;

namespace TRAFO.Repositories.BalanceWriting;

// todo #85
public interface IBalanceWriter
{
    void WriteBalance(Balance balance);
}
