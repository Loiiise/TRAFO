using TRAFO.IO.Command.Flags;
using TRAFO.IO.TransactionReading;
using TRAFO.IO.TransactionWriting;
using TRAFO.Logic.Categorization;
using TRAFO.Parsing;

namespace TRAFO.IO.Command;
public class LoadTransactionFileCommand : Command
{
    public LoadTransactionFileCommand(ITransactionStringReader transactionStringReader, IParser parser, ICategorizator[] categorizators, ITransactionWriter transactionWriter, string[] arguments, ICommandFlag[] flags) : base(arguments, flags)
    {
        _transactionStringReader = transactionStringReader;
        _parser = parser;
        _categorizators = categorizators;
        _transactionWriter = transactionWriter;
    }

    protected override int _expectedAmountOfArguments => 1;

    public override void Execute()
    {
        var transactionStrings = _transactionStringReader.ReadAllLines(Arguments[0], true);
        var transactions = _parser.Parse(transactionStrings);
        foreach (var categorizator in _categorizators)
        {
            transactions = categorizator.ApplyPredicates(transactions, Categories.GetDefaultPredicates());
        }
        _transactionWriter.WriteTransactions(transactions);
    }

    protected override void ValidateInternally()
    {
        if (!File.Exists(Arguments[0]))
        {
            throw new FileNotFoundException("File not found: " + Arguments[0]);
        }
    }

    protected override bool IsSupported(ICommandFlag flag)
    {
        throw new NotImplementedException();
    }

    private readonly ITransactionStringReader _transactionStringReader;
    private readonly IParser _parser;
    private readonly ICategorizator[] _categorizators;
    private readonly ITransactionWriter _transactionWriter;
}
