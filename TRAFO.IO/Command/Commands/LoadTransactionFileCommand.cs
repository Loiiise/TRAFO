using TRAFO.IO.Command.Arguments;
using TRAFO.IO.Command.Flags;
using TRAFO.IO.TransactionReading;
using TRAFO.IO.TransactionWriting;
using TRAFO.Logic.Categorization;
using TRAFO.Parsing;

namespace TRAFO.IO.Command;
public class LoadTransactionFileCommand : FromTillCommand
{
    public required FilePathArgument FilePathArgument { get; init; }

    public LoadTransactionFileCommand(ITransactionStringReader transactionStringReader, IParser parser, ICategorizator[] categorizators, ITransactionWriter transactionWriter, ICommandFlag[] flags) : base(flags)
    {
        _transactionStringReader = transactionStringReader;
        _parser = parser;
        _categorizators = categorizators;
        _transactionWriter = transactionWriter;
    }

    public override void Execute()
    {
        var transactionStrings = _transactionStringReader.ReadAllLines(FilePathArgument.Value, true);
        var transactions = _parser.Parse(transactionStrings);
            
        if (_from is not null) transactions = transactions.Where(t => t.Timestamp >= _from);
        if (_till is not null) transactions = transactions.Where(t => t.Timestamp <= _till);        

        foreach (var categorizator in _categorizators)
        {
            transactions = categorizator.ApplyPredicates(transactions, Categories.GetDefaultPredicates());
        }
        _transactionWriter.WriteTransactions(transactions);
    }

    private readonly ITransactionStringReader _transactionStringReader;
    private readonly IParser _parser;
    private readonly ICategorizator[] _categorizators;
    private readonly ITransactionWriter _transactionWriter;
}
