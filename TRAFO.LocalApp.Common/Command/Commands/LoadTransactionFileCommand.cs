using TRAFO.LocalApp.Common.Command.Arguments;
using TRAFO.LocalApp.Common.Command.Flags;
using TRAFO.Logic.Categorization;
using TRAFO.Services.Parser;
using TRAFO.Repositories.TransactionWriting;
using TRAFO.Repositories.TransactionReading;
using TRAFO.LocalApp.Common.FileReading;

namespace TRAFO.LocalApp.Common.Command;
public class LoadTransactionFileCommand : FromTillCommand
{
    public required FilePathArgument FilePathArgument { get; init; }

    public LoadTransactionFileCommand(ITransactionFileReader transactionFileReader, ILabelApplier[] labelers, ITransactionWriter transactionWriter, ICommandFlag[] flags) : base(flags)
    {
        _transactionFileReader = transactionFileReader;
        _labelers = labelers;
        _transactionWriter = transactionWriter;
    }

    public override void Execute()
    {
        var transactions = _transactionFileReader.ReadAllTransactions(FilePathArgument.Value);

        if (_from is not null) transactions = transactions.Where(t => t.Timestamp >= _from);
        if (_till is not null) transactions = transactions.Where(t => t.Timestamp <= _till);

        foreach (var categorizator in _labelers)
        {
            transactions = categorizator.ApplyPredicates(transactions, Labels.GetDefaultPredicates());
        }
        _transactionWriter.WriteTransactions(transactions);
    }

    private readonly ITransactionFileReader _transactionFileReader;
    private readonly ILabelApplier[] _labelers;
    private readonly ITransactionWriter _transactionWriter;
}
