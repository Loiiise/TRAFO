using TRAFO.IO.BalanceReading;
using TRAFO.IO.BalanceWriting;
using TRAFO.IO.TransactionReading;
using TRAFO.IO.TransactionWriting;

namespace TRAFO.IO.Database;
public interface IDatabase :
    ICategoryReader,
    ITransactionReader,
    ITransactionWriter,
    ITransactionLabelUpdater,
    IBalanceReader,
    IBalanceWriter
{
}
