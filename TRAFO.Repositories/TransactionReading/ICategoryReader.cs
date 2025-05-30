namespace TRAFO.Repositories.TransactionReading;

public interface ICategoryReader
{
    IEnumerable<string> GetAllCategories();
}
