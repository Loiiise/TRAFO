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
        }

        public static void DummyFlowFunction(
            ITransactionStringReader reader,
            IParser parser,
            ICategorizator categorizator,
            ITransactionWriter writer
            )
        {
            var stringLines = reader.ReadAllLines();

            var transactions = parser.Parse(stringLines);

            categorizator.ApplyPredicates(transactions, Array.Empty<Func<Transaction, Transaction>>());

            writer.WriteTransactions(transactions);
        }
    }
}