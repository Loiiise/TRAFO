using TRAFO.LocalApp.Common.Command.Arguments;
using TRAFO.LocalApp.Common.Command.Flags;
using TRAFO.LocalApp.Common.FileReading;
using TRAFO.Logic.Categorization;
using TRAFO.Logic.Extensions;
using TRAFO.Repositories;

namespace TRAFO.LocalApp.Common.Command;
public class LoadTransactionFileCommand : FromTillCommand
{
    public required FilePathArgument FilePathArgument { get; init; }
    private TransactionFileReaderConfiguration _transactionFileReaderConfiguration { get; init; }

    public LoadTransactionFileCommand(ITransactionFileReader transactionFileReader, ILabelApplier[] labelers, ITransactionWriter transactionWriter, ICommandFlag[] flags) : base(flags)
    {
        _transactionFileReader = transactionFileReader;
        _labelers = labelers;
        _transactionWriter = transactionWriter;

        var skipFirstLineFlag = flags.GetFirstOrDefault<SkipFirstLineFlag>();
        _transactionFileReaderConfiguration = skipFirstLineFlag == null ?
            new TransactionFileReaderConfiguration() :
            new TransactionFileReaderConfiguration()
            {
                SkipFirstLine = skipFirstLineFlag.Value,
            };
    }

    public override void Execute()
    {
        var transactions = _transactionFileReader.ReadAllTransactions(FilePathArgument.Value, _transactionFileReaderConfiguration);

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
