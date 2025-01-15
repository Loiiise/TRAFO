using TRAFO.Categorization;
using TRAFO.FileHandling;
using TRAFO.Logic;
using TRAFO.Parsing;

namespace TRAFO.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
