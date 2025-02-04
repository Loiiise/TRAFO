using TRAFO.IO.TransactionReading;
using TRAFO.IO.TransactionWriting;

namespace TRAFO.IO.Database;
public interface IDatabase : ITransactionReader, ITransactionWriter, ITransactionLabelUpdater
{
}
