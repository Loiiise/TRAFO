﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TRAFO.LocalApp.CLI.Command.Factory;
using TRAFO.LocalApp.CLI.Command.MetaData;
using TRAFO.LocalApp.Common.Command;
using TRAFO.LocalApp.Common.Command.Factory;
using TRAFO.LocalApp.Common.FileReading;
using TRAFO.Logic.Categorization;
using TRAFO.Repositories;
using TRAFO.Repositories.Repositories;
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
        builder.Services.AddSingleton<ITransactionFileReader, TransactionFileReader>();
        builder.Services.AddSingleton<ILabelApplier, TransactionLabelSetter>();

        var transactionRepository = new TransactionRepository();
        builder.Services.AddSingleton<ITransactionReader>(transactionRepository);
        builder.Services.AddSingleton<ITransactionWriter>(transactionRepository);

        var labelRepository = new LabelRepository();
        builder.Services.AddSingleton<ILabelReader>(labelRepository);
        builder.Services.AddSingleton<ITransactionLabelUpdater>(labelRepository);

        var balanceRepository = new BalanceRepository();
        builder.Services.AddSingleton<IBalanceReader>(balanceRepository);
        builder.Services.AddSingleton<IBalanceWriter>(balanceRepository);

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
}
