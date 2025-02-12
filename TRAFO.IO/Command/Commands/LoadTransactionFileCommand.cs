using TRAFO.IO.TransactionReading;
using TRAFO.IO.TransactionWriting;
using TRAFO.Parsing;

namespace TRAFO.IO.Command;
public class LoadTransactionFileCommand : Command
{
    public LoadTransactionFileCommand(ITransactionStringReader transactionStringReader, IParser parser, ITransactionWriter transactionWriter, string[] arguments) : base(arguments)
    {
        _transactionStringReader = transactionStringReader;
        _parser = parser;
        _transactionWriter = transactionWriter;
    }

    protected override int _expectedAmountOfArguments => 1;

    public override void Execute()
    {
        var transactionStrings = _transactionStringReader.ReadAllLines(Arguments[0], true);
        var transactions = _parser.Parse(transactionStrings);
        _transactionWriter.WriteTransactions(transactions);
    }

    protected override void ValidateInternally()
    {
        if (!File.Exists(Arguments[0]))
        {
            throw new FileNotFoundException("File not found: " + Arguments[0]);
        }
    }

    private readonly ITransactionStringReader _transactionStringReader;
    private readonly IParser _parser;
    private readonly ITransactionWriter _transactionWriter;
}
