using TRAFO.IO.Database;
using TRAFO.IO.TransactionReading;
using TRAFO.IO.TransactionWriting;
using TRAFO.Logic;
using TRAFO.Logic.Categorization;
using TRAFO.Parsing;

namespace TRAFO.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dummyHardcodedParser = new CustomCSVParser(6, 1, 0, null, 8, 9, 4, 15, 2, 19, "\",\"");

            var fileReader = new FileReader();
            var transactionStrings = fileReader.ReadAllLines("C:\\tmp\\transactionsDummy.csv", true).ToArray();

            var transaction = dummyHardcodedParser.Parse(transactionStrings.First());

            IDatabase database = new EntityFrameworkDatabase();
            database.WriteTransaction(transaction);

            var savedTransactions = database.ReadAllTransactions();
        }

        public static void DummyFlowFunction(
            ITransactionStringReader reader,
            IParser parser,
            ICategorizator categorizator,
            ITransactionWriter writer
            )
        {
            var stringLines = reader.ReadAllLines(string.Empty);

            var transactions = parser.Parse(stringLines);

            categorizator.ApplyPredicates(transactions, Array.Empty<Func<Transaction, Transaction>>());

            writer.WriteTransactions(transactions);
        }
    }
}