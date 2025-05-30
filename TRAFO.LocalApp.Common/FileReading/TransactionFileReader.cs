using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using TRAFO.Logic;
using TRAFO.Services.Parser;

namespace TRAFO.LocalApp.Common.FileReading;

public record TransactionFileReaderConfiguration
{
    public bool SkipFirstLine { get; init; } = true;
}

public class TransactionFileReader : ITransactionFileReader
{
    public TransactionFileReader(ILogger<TransactionFileReader> logger, IParser parser)
    {
        _logger = logger;
        _parser = parser;
    }

    public IEnumerable<Transaction> ReadAllTransactions(string path) => ReadAllTransactions(path, new TransactionFileReaderConfiguration());
    public IEnumerable<Transaction> ReadAllTransactions(string path, TransactionFileReaderConfiguration configuration)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(path);
        }

        foreach (var line in configuration.SkipFirstLine
            ? File.ReadAllLines(path).Skip(1)
            : File.ReadAllLines(path))
        {
            yield return _parser.Parse(line);
        }
    }

    public bool TryReadAllTransactions(string path, [NotNullWhen(true)] out IEnumerable<Transaction> transactions)
        => TryReadAllTransactions(path, new TransactionFileReaderConfiguration(), out transactions);
    public bool TryReadAllTransactions(string path, TransactionFileReaderConfiguration configuration, [NotNullWhen(true)] out IEnumerable<Transaction> transactions)
    {
        try
        {
            transactions = ReadAllTransactions(path, configuration);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error reading transacions from file: {Error}", ex.Message);
            transactions = Array.Empty<Transaction>();
            return false;
        }
    }

    private readonly ILogger<TransactionFileReader> _logger;
    private readonly IParser _parser;
}
