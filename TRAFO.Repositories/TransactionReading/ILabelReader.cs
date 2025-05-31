using TRAFO.Logic.Dto;

namespace TRAFO.Repositories.TransactionReading;

public interface ILabelReader
{
    IEnumerable<Label> GetAllLabels();
}
