using TRAFO.Repositories.BalanceReading;
using TRAFO.Repositories.BalanceWriting;
using TRAFO.Repositories.TransactionReading;
using TRAFO.Repositories.TransactionWriting;

namespace TRAFO.Repositories.Database;
public interface IDatabase :
    ILabelReader,
    ITransactionReader,
    ITransactionWriter,
    ITransactionLabelUpdater,
    IBalanceReader,
    IBalanceWriter
{
}
