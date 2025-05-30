using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TRAFO.LocalApp.CLI.Command.Factory;
using TRAFO.LocalApp.CLI.Command.MetaData;
using TRAFO.LocalApp.Common.Command;
using TRAFO.LocalApp.Common.Command.Factory;
using TRAFO.LocalApp.Common.TransactionReading;
using TRAFO.Logic.Categorization;
using TRAFO.Logic.Categorization.Predicates;
using TRAFO.Repositories.BalanceReading;
using TRAFO.Repositories.BalanceWriting;
using TRAFO.Repositories.Database;
using TRAFO.Repositories.TransactionReading;
using TRAFO.Repositories.TransactionWriting;
using TRAFO.Services.Parser;
using TRAFO.Services.Parser.CSV;

namespace TRAFO.LocalApp.CLI;

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
        builder.Services.AddSingleton<ILabelReader>(database);
        builder.Services.AddSingleton<IBalanceReader>(database);
        builder.Services.AddSingleton<IBalanceWriter>(database);
        builder.Services.AddSingleton<ITransactionWriter>(database);
        builder.Services.AddSingleton<ITransactionLabelUpdater>(database);

        builder.Services.AddSingleton<ICommandMetaData, CommandMetaData>();
        builder.Services.AddSingleton<IFlagMetaData, FlagMetaData>();

        builder.Services.AddSingleton<ICommandFactory, CommandFactory>();
        builder.Services.AddSingleton<ICommandArgumentFactory, CommandArgumentFactory>();
        builder.Services.AddSingleton<ICommandFlagFactory, CommandFlagFactory>();

        builder.Services.AddSingleton<ICommandFlagStringFactory, CommandFlagStringFactory>();
        builder.Services.AddSingleton<ICommandStringFactory, CommandStringFactory>();

        builder.Services.AddHostedService(services => new UserCommandHandler(
            services.GetService<ICommandStringFactory>()!,
            services.GetService<IBasicUserInputHandler>()!,
            services.GetService<IUserCommunicationHandler>()!,
            services.GetService<IBasicUserOutputHandler>()!,
            services.GetService<ICommandMetaData>()!,
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
