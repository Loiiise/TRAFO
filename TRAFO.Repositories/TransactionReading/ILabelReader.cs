namespace TRAFO.Repositories.TransactionReading;

public interface ILabelReader
{
    IEnumerable<string> GetAllLabels();
}
