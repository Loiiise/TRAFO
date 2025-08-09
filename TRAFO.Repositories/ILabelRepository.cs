using TRAFO.Logic.Dto;

namespace TRAFO.Repositories;

internal interface ILabelRepository :
    ILabelReader,
    ITransactionLabelUpdater
{ }

public interface ILabelReader
{
    IEnumerable<Label> GetAllLabels();
    Label? TryGetLabelById(Guid labelId);
    IEnumerable<Label> TryGetLabelById(IEnumerable<Guid> labelIds);
}

public interface ITransactionLabelUpdater
{
    public void SetLabel(Transaction transaction, Label label);
    public void UpdateLabels(Transaction transaction);
}
