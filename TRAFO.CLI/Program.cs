using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TRAFO.IO.Command;
using TRAFO.IO.Database;
using TRAFO.IO.TransactionReading;
using TRAFO.IO.TransactionWriting;
using TRAFO.Logic;
using TRAFO.Logic.Categorization;
using TRAFO.Logic.Categorization.Predicates;
using TRAFO.Parsing;

namespace TRAFO.CLI;

internal class Program
{
    static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        var consoleUserInputHandler = new ConsoleUserInputHandler();
        builder.Services.AddSingleton<IUserCommunicationHandler>(consoleUserInputHandler);
        builder.Services.AddSingleton<IBasicUserInputHandler>(consoleUserInputHandler);
        builder.Services.AddSingleton<IBasicUserOutputHandler>(consoleUserInputHandler);

        builder.Services.AddSingleton<IParser>(new CustomCSVParser(6, 1, 0, null, 8, 9, 4, 15, 2, 19, "\",\""));
        builder.Services.AddSingleton<ITransactionStringReader, FileReader>();
        builder.Services.AddSingleton<ICategorizator, PrimairyLabelSetter>();

        var database = new EntityFrameworkDatabase();
        builder.Services.AddSingleton<IDatabase>(database);
        builder.Services.AddSingleton<ITransactionReader>(database);
        builder.Services.AddSingleton<ITransactionWriter>(database);
        builder.Services.AddSingleton<ITransactionLabelUpdater>(database);

        builder.Services.AddSingleton<ICommandFactory, CommandFactory>();
        
        builder.Services.AddHostedService(services => new UserCommandHandler(
            services.GetService<ICommandFactory>()!,
            services.GetService<IBasicUserInputHandler>()!,
            services.GetService<IUserCommunicationHandler>()!,
            args
            ));

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

        categorizator.ApplyPredicates(transactions, Array.Empty<TransactionPredicate>());

        writer.WriteTransactions(transactions);
    }


}
