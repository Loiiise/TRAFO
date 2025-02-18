namespace TRAFO.IO.TransactionReading;

public interface ICategoryReader
{
    IEnumerable<string> GetAllCategories();
}
