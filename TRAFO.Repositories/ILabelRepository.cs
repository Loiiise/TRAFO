using TRAFO.Logic.Dto;

namespace TRAFO.Repositories;

public interface ILabelRepository :
    ILabelWriter,
    ILabelReader,
    ITransactionLabelUpdater
{ }

public interface ILabelWriter
{
    // todo #86: check if this still makes sense
    void CreateIfNotExists(IEnumerable<string> labelNames);
    void CreateIfNotExists(IEnumerable<Label> label);
}

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
