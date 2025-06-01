using TRAFO.Logic.Dto;

namespace TRAFO.Repositories;

internal interface ILabelRepository :
    ILabelReader,
    ITransactionLabelUpdater
{ }

public interface ILabelReader
{
    IEnumerable<Label> GetAllLabels();
}

public interface ITransactionLabelUpdater
{
    public void SetLabel(Transaction transaction, Label label);
    public void UpdateLabels(Transaction transaction);
}
