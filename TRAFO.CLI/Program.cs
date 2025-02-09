using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TRAFO.IO.Command;
using TRAFO.IO.Database;
using TRAFO.IO.TransactionReading;
using TRAFO.IO.TransactionWriting;
using TRAFO.Logic;
using TRAFO.Logic.Categorization;
using TRAFO.Parsing;

namespace TRAFO.CLI;

internal class Program
{
    static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHostedService<UserCommandHandler>();
        builder.Services.AddSingleton<ICommandFactory, CommandFactory>();
        builder.Services.AddSingleton<IBasicUserInputHandler, ConsoleUserInputHandler>();
        builder.Services.AddSingleton<IUserCommunicationHandler>(services =>
        {
            if (services.GetService<IBasicUserInputHandler>() is IUserCommunicationHandler userCommunicationHandler)
            {
                return userCommunicationHandler;
            }
            throw new NotImplementedException();
        });
        builder.Services.AddSingleton<IParser>(new CustomCSVParser(6, 1, 0, null, 8, 9, 4, 15, 2, 19, "\",\""));
        builder.Services.AddSingleton<ITransactionStringReader, FileReader>();
        builder.Services.AddSingleton<IDatabase, EntityFrameworkDatabase>();

        using IHost host = builder.Build();

        host.Run();
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
